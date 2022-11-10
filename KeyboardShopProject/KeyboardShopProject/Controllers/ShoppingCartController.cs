using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Keyboard.ShopProject.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _cartService;
        private readonly CheckForStatusCode _check;

        public ShoppingCartController(IShoppingCartService cartService, CheckForStatusCode check)
        {
            _cartService = cartService;
            _check = check;
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int clientId)
        {
            var response = await _cartService.GetContent(clientId);
            return _check.CheckShoppingCartResponse(response.StatusCode, response);
        }

        [HttpPost(nameof(AddContent))]
        public async Task<IActionResult> AddContent(ShoppingCartRequest cart)
        {
            var response = await _cartService.AddToShoppingCard(cart);
            return _check.CheckShoppingCartResponse(response.StatusCode, response);
        }

        [HttpDelete(nameof(DeleteKeyboardFromShoppingCart))]
        public async Task<IActionResult> DeleteKeyboardFromShoppingCart(ShoppingCartRequest cart)
        {
            var response = await _cartService.RemoveFromShoppingCart(cart);
            return _check.CheckShoppingCartResponse(response.StatusCode, response);
        }

        [Authorize]
        [HttpDelete(nameof(EmptyShoppingCart))]
        public async Task<IActionResult> EmptyShoppingCart(int clientId)
        {
            var response = await _cartService.EmptyShoppingCart(clientId);
            return _check.CheckShoppingCartResponse(response.StatusCode, response);
        }
    }
}
