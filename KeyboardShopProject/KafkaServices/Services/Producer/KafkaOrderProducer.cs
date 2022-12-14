using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using Keyboard.Models.Models;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Producer
{
    public class KafkaOrderProducer : BaseKafkaProducer<Guid, KafkaReportModelForOrder>
    {
        public ProducerConfig Config { get; set; }
        public IOptionsMonitor<KafkaSettingsForOrder> Settings { get; set; }
        public KafkaOrderProducer(IOptionsMonitor<KafkaSettingsForOrder> settings)
        {
            Settings = settings;
            Config = new ProducerConfig()
            {
                BootstrapServers = settings.CurrentValue.BootstrapServers,
            };
        }
    }
}
