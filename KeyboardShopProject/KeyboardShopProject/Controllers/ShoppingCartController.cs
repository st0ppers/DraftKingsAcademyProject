using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int clientId)
        {
            return Ok(await _cartService.GetContent(clientId));
        }

        [HttpPost(nameof(AddContent))]
        public async Task<IActionResult> AddContent(ShoppingCartRequest cart)
        {
            return Ok(await _cartService.AddToShoppingCard(cart));
        }

        [HttpDelete(nameof(DeleteKeyboardFromShoppingCart))]
        public async Task<IActionResult> DeleteKeyboardFromShoppingCart(ShoppingCartRequest cart)
        {
            return Ok(await _cartService.RemoveFromShoppingCart(cart));
        }

        [HttpDelete(nameof(EmptyShoppingCart))]
        public async Task<IActionResult> EmptyShoppingCart(int clientId)
        {
            return Ok(await _cartService.EmptyShoppingCart(clientId));
        }
    }
}
