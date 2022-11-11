using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IShoppingCartMongoRepository
    {
        public Task<ShoppingCartModel> GetContent(int clientID);
        public Task<ShoppingCartModel> CreateShoppingCart(ShoppingCartModel cart);
        public Task<ShoppingCartModel> AddToShoppingCard(ShoppingCartModel request);
        public Task<ShoppingCartModel> RemoveFromShoppingCart(ShoppingCartModel request);
        public Task<ShoppingCartModel> EmptyShoppingCart(int clientID);
    }
}
