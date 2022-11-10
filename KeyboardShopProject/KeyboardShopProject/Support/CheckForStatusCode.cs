using Keyboard.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Keyboard.ShopProject.Support
{
    public class CheckForStatusCode :ControllerBase
    {
        public IActionResult CheckClientResponse(HttpStatusCode code, ClientResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        public IActionResult CheckKeyboardResponse(HttpStatusCode code, KeyboardResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        public IActionResult CheckOrderResponse(HttpStatusCode code, OrderResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        public IActionResult CheckShoppingCartResponse(HttpStatusCode code, ShoppingCartResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
