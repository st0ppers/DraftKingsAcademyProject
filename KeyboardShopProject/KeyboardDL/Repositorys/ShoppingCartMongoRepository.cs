using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Keyboard.DL.Repositorys
{
    public class ShoppingCartMongoRepository : IShoppingCartMongoRepository
    {
        private readonly IOptionsMonitor<MongoConfiguration> _mongnSettings;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _databse;
        private readonly IMongoCollection<ShoppingCartModel> _collection;

        public ShoppingCartMongoRepository(IOptionsMonitor<MongoConfiguration> mongnSettings)
        {
            _mongnSettings = mongnSettings;
            _client = new MongoClient(_mongnSettings.CurrentValue.ConnecionString);
            _databse = _client.GetDatabase(_mongnSettings.CurrentValue.DatabaseName);
            _collection = _databse.GetCollection<ShoppingCartModel>(_mongnSettings.CurrentValue.CollectionName);
        }

        public async Task<ShoppingCartModel> GetContent(int clientID)
        {
            var cart =await _collection.FindAsync(x => x.ClientId == clientID);
            return cart.FirstOrDefault();
        }

        public async Task<ShoppingCartModel> AddToShoppingCard(ShoppingCartModel cart)
        {
            await _collection.InsertOneAsync(cart);
            return cart;
        }

        public Task<ShoppingCartModel> RemoveFromShoppingCart(ShoppingCartModel cart)
        {
            throw new NotImplementedException();
        }

        public Task EmptyShoppingCart(int clientID)
        {
            throw new NotImplementedException();
        }
    }
}
