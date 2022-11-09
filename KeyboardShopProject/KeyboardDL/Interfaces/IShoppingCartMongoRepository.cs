using Keyboard.Models.Models;
using Keyboard.Models.Requests;

namespace Keyboard.DL.Interfaces
{
    public interface IShoppingCartMongoRepository
    {
        public Task<ShoppingCartModel> GetContent(int clientID);
        public Task<ShoppingCartModel> CreateShoppingCart(ShoppingCartModel cart);
        public Task<ShoppingCartModel> AddToShoppingCard(ShoppingCartRequest request);
        public Task<ShoppingCartModel> RemoveFromShoppingCart(ShoppingCartRequest request);
        public Task<ShoppingCartModel> EmptyShoppingCart(int clientID);
    }
}
