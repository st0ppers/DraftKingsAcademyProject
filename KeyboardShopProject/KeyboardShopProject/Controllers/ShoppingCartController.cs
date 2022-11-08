using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartMongoRepository _shoppingCartMongoRepository;

        public ShoppingCartController(IShoppingCartMongoRepository shoppingCartMongoRepository)
        {
            _shoppingCartMongoRepository = shoppingCartMongoRepository;
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int id)
        {
            return Ok(await _shoppingCartMongoRepository.GetContent(id));
        }

        [HttpPost(nameof(AddContent))]
        public async Task<IActionResult> AddContent(ShoppingCartModel cart)
        {
            return Ok(await _shoppingCartMongoRepository.AddToShoppingCard(cart));
        }
    }
}
