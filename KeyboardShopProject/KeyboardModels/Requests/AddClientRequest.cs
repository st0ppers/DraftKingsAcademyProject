using MessagePack;

namespace Keyboard.Models.Requests
{
    [MessagePackObject]
    public class AddClientRequest
    {
        [Key(0)]
        public string FullName { get; set; }
        [Key(1)]
        public string Address { get; set; }
        [Key(2)]
        public int Age { get; set; }
    }
}
