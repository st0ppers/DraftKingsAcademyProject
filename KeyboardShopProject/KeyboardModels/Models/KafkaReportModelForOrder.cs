using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject()]
    public class KafkaReportModelForOrder 
    {
        [Key(0)]
        public Guid OrderID { get; set; }
        [Key(1)]
        public decimal TotalPrice { get; set; }
        [Key(2)]
        public DateTime Date { get; set; }
        [Key(3)]
        public List<KeyboardModel> Keyboards { get; set; }

    }
}
