using Confluent.Kafka;
using KafkaServices.Serializers;

namespace KafkaServices.Services.Producer
{
    public class BaseKafkaProducer<TKey, TValue>
    {
        public async Task Produce(TKey key, TValue value, string topic, ProducerConfig config)
        {
            var producer = new ProducerBuilder<TKey, TValue>(config).SetKeySerializer(new MsgPackSerializer<TKey>())
                .SetValueSerializer(new MsgPackSerializer<TValue>()).Build();
            try
            {
                var msg = new Message<TKey, TValue>()
                {
                    Key = key,
                    Value = value
                };
                var result = await producer.ProduceAsync(topic, msg);
                if (result != null)
                {
                    Console.WriteLine($"Delivered key {result.Key}");
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
