using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using Keyboard.Models.Requests;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Producer
{
    public class KafkaClientProducer : BaseKafkaProducer<int, AddClientRequest>
    {
        public ProducerConfig Config { get; set; }
        public IOptionsMonitor<KafkaSettingsForClient> Settings { get; set; }
        public KafkaClientProducer(IOptionsMonitor<KafkaSettingsForClient> settings)
        {
            Settings = settings;
            Config = new ProducerConfig()
            {
                BootstrapServers = settings.CurrentValue.BootstrapServers,
            };
        }
    }
}
