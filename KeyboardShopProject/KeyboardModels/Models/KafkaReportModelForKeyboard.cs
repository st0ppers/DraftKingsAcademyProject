using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject()]
    public class KafkaReportModelForKeyboard
    {
        [Key(0)]
        public string Size { get; set; }
        [Key(1)]
        public string Model { get; set; }
        [Key(2)]
        public decimal Price { get; set; }
        [Key(3)]
        public int Quantity { get; set; }
        [Key(4)]
        public string Color { get; set; }
    }
}
