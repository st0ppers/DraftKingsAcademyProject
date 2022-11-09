using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject()]
    public class KafkaReportModel
    {
        [Key(0)]
        public int OrderID { get; set; }
        [Key(1)]
        public decimal TotalPrice { get; set; }
        [Key(2)]
        public DateTime DateOfOrder { get; set; }
        [Key(3)]
        public List<KeyboardModel> Keyboard { get; set; }

    }
}
