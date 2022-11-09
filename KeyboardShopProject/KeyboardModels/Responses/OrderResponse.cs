using Keyboard.Models.Models;
using MessagePack;

namespace Keyboard.Models.Responses
{
    public class OrderResponse : BaseResponse
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOfOrder { get; set; }
        public List<KeyboardModel> Keyboard { get; set; }
    }
}
