using System.Threading.Tasks.Dataflow;
using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using KafkaServices.Serializers;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Consumer
{
    public class KafkaOrderConsumer<TKey, TValue>  /*where TValue : IGetId*/
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly TransformBlock<TValue, string> _transformBlock;
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;

        public KafkaOrderConsumer(IOptionsMonitor<KafkaSettingsForOrder> settings, IKeyboardSqlRepository keyboardSqlRepository)
        {
            _keyboardSqlRepository = keyboardSqlRepository;
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

            _transformBlock = new TransformBlock<TValue, string>(async request =>
            {
                //var keyboard = await _keyboardSqlRepository.GetById(request.Get());
                return $"Ordered successfull";//{keyboard.Model}
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
