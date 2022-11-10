using Keyboard.Models.Models;

namespace Keyboard.Models.Requests
{
    public class UpdateOrderRequest
    {
        public Guid OrderID { get; set; }
        public List<KeyboardModel> Keyboards { get; set; }
        public int ClientID { get; set; }
        public DateTime Date { get; set; }
    }
}
