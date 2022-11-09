using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject]
    public class KafkaReportModelForClient
    {
        [Key(0)]
        public string FullName { get; set; }
        [Key(1)]
        public string Address { get; set; }
        [Key(2)]
        public int Age { get; set; }
    }
}
