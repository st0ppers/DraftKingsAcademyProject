using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IOrderSqlRepository
    {
        public Task<IEnumerable<OrderModel>> GetAllOrders();
        public Task<OrderModel> GetById(int id);
        public Task<OrderModel> CreateOrder(OrderModel order);
        public Task<OrderModel> UpdateOrder(OrderModel order);
        public Task<OrderModel> DeleteOrder(int id);
    }
}
