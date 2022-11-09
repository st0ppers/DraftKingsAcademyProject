using MessagePack;

namespace Keyboard.Models.Requests
{
    [MessagePackObject()]
    public class AddOrderRequest /*: IGetId*/
    {
        //[Key(0)]
        //public int KeyboardID { get; set; }
        [Key(0)]
        public int ClientID { get; set; }
        [Key(1)]
        public DateTime Date { get; set; }
        //public int Get()
        //{
        //    return KeyboardID;
        //}
    }
}
