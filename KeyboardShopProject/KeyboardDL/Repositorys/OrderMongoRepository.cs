using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Keyboard.DL.Repositorys
{
    public class OrderMongoRepository : IOrderMongoRepository
    {
        private readonly IOptionsMonitor<MongoConfiguration> _mongnSettings;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _databse;
        private readonly IMongoCollection<OrderModel> _collection;

        public OrderMongoRepository(IOptionsMonitor<MongoConfiguration> mongnSettings, IKeyboardSqlRepository keyboardSqlRepository)
        {
            _mongnSettings = mongnSettings;
            _client = new MongoClient(_mongnSettings.CurrentValue.ConnecionString);
            _databse = _client.GetDatabase(_mongnSettings.CurrentValue.DatabaseName);
            _collection = _databse.GetCollection<OrderModel>(_mongnSettings.CurrentValue.CollectionNmaeForOrder);
        }

        public async Task<OrderModel> GetOrder(Guid orderId)
        {
            var order = await _collection.FindAsync(x => x.OrderID == orderId);
            return await order.FirstOrDefaultAsync();
        }

        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            await _collection.InsertOneAsync(order);
            return order;
        }

        public async Task<OrderModel> UpdateOrder(OrderModel order)
        {
            await _collection.ReplaceOneAsync(x => x.OrderID == order.OrderID, order);
            var result = await GetOrder(order.OrderID);
            return result;
        }

        public async Task<OrderModel> DeleteOrder(Guid orderId)
        {
            var order = await GetOrder(orderId);
            await _collection.DeleteOneAsync(x => x.OrderID == orderId);
            return order;
        }
    }
}
