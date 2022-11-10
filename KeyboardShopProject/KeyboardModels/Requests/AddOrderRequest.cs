using Keyboard.Models.Models;

namespace Keyboard.Models.Requests
{
    public class AddOrderRequest 
    {
        public List<KeyboardModel> Keyboards { get; set; }
        public int ClientID { get; set; }
        public DateTime Date { get; set; }
    }
}
