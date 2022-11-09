using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<ShoppingCartResponse> GetContent(int clientID);
        public Task<ShoppingCartResponse> AddToShoppingCard(ShoppingCartRequest request);
        public Task<ShoppingCartResponse> RemoveFromShoppingCart(ShoppingCartRequest request);
        public Task<ShoppingCartResponse> EmptyShoppingCart(int clientID);
    }
}
