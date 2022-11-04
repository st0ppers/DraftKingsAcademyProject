using Keyboard.Models.Models;

namespace Keyboard.Models.Responses
{
    public class OrderResponse : BaseResponse
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOfOrder { get; set; }
        public KeyboardModel Keyboard { get; set; }
        public ClientModel Client { get; set; }
    }
}
