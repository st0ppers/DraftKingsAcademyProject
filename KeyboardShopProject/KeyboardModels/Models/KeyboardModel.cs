using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject]
    public class KeyboardModel
    {
        [Key(0)]
        public int KeyboardID { get; set; }
        [Key(1)]
        public string Size { get; set; }
        [Key(2)]
        public string Model { get; set; }
        [Key(3)]
        public decimal Price { get; set; }
        [Key(4)]
        public int Quantity { get; set; }
        [Key(5)]
        public string Color { get; set; }
    }
}