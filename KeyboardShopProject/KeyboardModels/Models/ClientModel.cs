using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject()]
    public class ClientModel
    {
        [Key(0)]
        public int ClientID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
    }
}
