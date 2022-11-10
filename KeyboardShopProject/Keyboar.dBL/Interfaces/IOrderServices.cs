using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Interfaces
{
    public interface IOrderServices
    {
        public Task<OrderResponse> GetById(Guid id);
        public Task<OrderResponse> CreateOrder(int orderId);
        public Task<OrderResponse> UpdateOrder(UpdateOrderRequest request);
        public Task<OrderResponse> DeleteOrder(Guid id);
    }
}
