using Confluent.Kafka;
using KafkaServices.KafkaSettings;
using Keyboard.Models.Models;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Producer
{
    public class KafkaKeyboardProducer : BaseKafkaProducer<int, KafkaReportModelForKeyboard>
    {
        public ProducerConfig Config { get; set; }
        public IOptionsMonitor<KafkaSettingsForKeyboard> Settings { get; set; }
        public KafkaKeyboardProducer(IOptionsMonitor<KafkaSettingsForKeyboard> settings)
        {
            Settings = settings;
            Config = new ProducerConfig()
            {
                BootstrapServers = settings.CurrentValue.BootstrapServers,
            };
        }
    }
}
