using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Interfaces
{
    public interface IOrderServices
    {
        public Task<IEnumerable<OrderModel>> GetAllOrders();
        public Task<OrderModel> GetById(int id);
        public Task<OrderResponse> CreateOrder(AddOrderRequest request);
        public Task<OrderResponse> UpdateOrder(UpdateOrderRequest request);
        public Task<OrderResponse> DeleteOrder(int id);
    }
}
