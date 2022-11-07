using Keyboard.Models.Models;
using MessagePack;

namespace Keyboard.Models.Requests
{
    [MessagePackObject()]
    public class AddOrderRequest : IGetId
    {
        [Key(0)]
        public int KeyboardID { get; set; }
        [Key(1)]
        public int ClientID { get; set; }
        [Key(2)]
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"KeyboardID = {KeyboardID}, ClientID = {ClientID}";
        }

        public int Get()
        {
            return KeyboardID;
        }
    }
}
