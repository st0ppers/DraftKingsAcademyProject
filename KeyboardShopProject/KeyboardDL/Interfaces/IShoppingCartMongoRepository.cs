using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IShoppingCartMongoRepository
    {
        public Task<ShoppingCartModel> GetContent(int clientID);
        public Task<ShoppingCartModel> AddToShoppingCard(ShoppingCartModel cart);
        public Task<ShoppingCartModel> RemoveFromShoppingCart(ShoppingCartModel cart);
        public Task EmptyShoppingCart(int clientID);
    }
}
