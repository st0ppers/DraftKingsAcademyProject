using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet(nameof(GetAllOrders))]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderServices.GetAllOrders());
        }

        [HttpGet(nameof(GetOrderById))]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return Ok(await _orderServices.GetById(id));
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<IActionResult> CreateOrder([FromBody] AddOrderRequest request)
        {
            return Ok(await _orderServices.CreateOrder(request));
        }

        [HttpPut(nameof(UpdateOrder))]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            return Ok(await _orderServices.UpdateOrder(request));
        }

        [HttpDelete(nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return Ok(await _orderServices.DeleteOrder(id));
        }
    }
}
