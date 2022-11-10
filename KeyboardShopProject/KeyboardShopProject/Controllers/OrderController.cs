using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Keyboard.ShopProject.Support;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly CheckForStatusCode _check;

        public OrderController(IOrderServices orderServices, CheckForStatusCode check)
        {
            _orderServices = orderServices;
            _check = check;
        }

        [HttpGet(nameof(GetOrderById))]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var response = await _orderServices.GetById(id);
            return _check.CheckOrderResponse(response.StatusCode, response);
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<IActionResult> CreateOrder(int clientId)
        {
            var response = await _orderServices.CreateOrder(clientId);
            return _check.CheckOrderResponse(response.StatusCode, response);
        }

        [HttpPut(nameof(UpdateOrder))]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            var response = await _orderServices.UpdateOrder(request);
            return _check.CheckOrderResponse(response.StatusCode, response);
        }

        [HttpDelete(nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var response = await _orderServices.DeleteOrder(id);
            return _check.CheckOrderResponse(response.StatusCode, response);
        }
    }
}
