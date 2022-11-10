using Keyboard.Models.Models;

namespace Keyboard.Models.Responses
{
    public class OrderResponse : BaseResponse
    {
        public OrderModel Order { get; set; }
    }
}
