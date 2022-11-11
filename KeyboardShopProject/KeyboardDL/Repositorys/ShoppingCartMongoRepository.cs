using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
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
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;

        public ShoppingCartMongoRepository(IOptionsMonitor<MongoConfiguration> mongnSettings, IKeyboardSqlRepository keyboardSqlRepository)
        {
            _mongnSettings = mongnSettings;
            _keyboardSqlRepository = keyboardSqlRepository;
            _client = new MongoClient(_mongnSettings.CurrentValue.ConnecionString);
            _databse = _client.GetDatabase(_mongnSettings.CurrentValue.DatabaseName);
            _collection = _databse.GetCollection<ShoppingCartModel>(_mongnSettings.CurrentValue.CollectionName);
        }

        public async Task<ShoppingCartModel> GetContent(int clientID)
        {
            var cart = await _collection.FindAsync(x => x.ClientId == clientID);
            return await cart.FirstOrDefaultAsync();
        }

        public async Task<ShoppingCartModel> CreateShoppingCart(ShoppingCartModel cart)
        {
            await _collection.InsertOneAsync(cart);
            return cart;
        }

        public async Task<ShoppingCartModel> AddToShoppingCard(ShoppingCartModel request)
        {
            await _collection.ReplaceOneAsync(x => x.ClientId == request.ClientId, request);
            return request;
        }

        public async Task<ShoppingCartModel> RemoveFromShoppingCart(ShoppingCartModel request)
        {
            await _collection.ReplaceOneAsync(x => x.ClientId == request.ClientId, request);
            return request;
        }

        public async Task<ShoppingCartModel> EmptyShoppingCart(int clientID)
        {
            var cart = await GetContent(clientID);
            cart.Keyboards.Clear();
            await _collection.DeleteOneAsync(x => x.ClientId == clientID);
            return cart;
        }
    }
}
