using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using KafkaServices.Serializers;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services
{
    public class KafkaClientProducer<TKey, TValue>
    {
        private readonly ProducerConfig _config;
        private readonly IOptionsMonitor<KafkaSettingsForClient> _kafkaSettings;

        public KafkaClientProducer(IOptionsMonitor<KafkaSettingsForClient> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            _config = new ProducerConfig()
            {
                BootstrapServers = _kafkaSettings.CurrentValue.BootstrapServers
            };
        }
        public async Task Produce(TKey key, TValue value)
        {
            var producer = new ProducerBuilder<TKey, TValue>(_config).SetKeySerializer(new MsgPackSerializer<TKey>())
                .SetValueSerializer(new MsgPackSerializer<TValue>()).Build();
            try
            {
                var msg = new Message<TKey, TValue>()
                {
                    Key = key,
                    Value = value
                };
                var result = await producer.ProduceAsync(_kafkaSettings.CurrentValue.Topic, msg);
                if (result != null)
                {
                    Console.WriteLine($"Delivered key {result.Key} with value {result.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
