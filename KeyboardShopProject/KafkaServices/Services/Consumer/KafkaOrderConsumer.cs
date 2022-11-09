using System.Threading.Tasks.Dataflow;
using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using KafkaServices.Serializers;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Consumer
{
    public class KafkaOrderConsumer<TKey, TValue> 
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly TransformBlock<TValue, string> _transformBlock;

        public KafkaOrderConsumer(IOptionsMonitor<KafkaSettingsForOrder> settings)
        {
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = settings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = settings.CurrentValue.GroupId
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(consumerConfig)
                .SetKeyDeserializer(new MsgPackDeserializer<TKey>())
                .SetValueDeserializer(new MsgPackDeserializer<TValue>()).Build();

            _consumer.Subscribe(settings.CurrentValue.Topic);

            _transformBlock = new TransformBlock<TValue, string>(request =>
            {
                return $"Ordered successfull";
            });
            var actionBlock = new ActionBlock<string>(Console.WriteLine);

            _transformBlock.LinkTo(actionBlock);
        }

        public async Task<TValue?> Consume()
        {
            var orderRequest = _consumer.Consume().Message.Value;
            await _transformBlock.SendAsync(orderRequest);
            return orderRequest;
        }
    }
}
