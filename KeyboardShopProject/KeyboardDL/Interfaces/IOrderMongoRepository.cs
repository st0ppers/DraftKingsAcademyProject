using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IOrderMongoRepository
    {
        public Task<OrderModel> GetOrder(Guid orderId);
        public Task<OrderModel> CreateOrder(OrderModel order);
        public Task<OrderModel> UpdateOrder(OrderModel order);
        public Task<OrderModel> DeleteOrder(Guid orderId);
    }
}
