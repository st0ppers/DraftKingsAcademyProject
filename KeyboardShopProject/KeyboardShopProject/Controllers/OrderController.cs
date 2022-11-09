using System.Net;
using KafkaServices.KafkaSettings;
using KafkaServices.Services.Producer;
using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly KafkaOrderProducer _kafkaProducer;

        public OrderController(IOrderServices orderServices,IOptionsMonitor<KafkaSettingsForOrder> settings)
        {
            _orderServices = orderServices;
            _kafkaProducer = new KafkaOrderProducer(settings);
        }

        [HttpGet(nameof(GetAllOrders))]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderServices.GetAllOrders());
        }

        [HttpGet(nameof(GetOrderById))]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _orderServices.GetById(id);
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<IActionResult> CreateOrder([FromBody] AddOrderRequest request)
        {
            var response = await _orderServices.CreateOrder(request);
            await _kafkaProducer.Produce(response.OrderID, response,_kafkaProducer.Settings.CurrentValue.Topic,_kafkaProducer.Config);
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpPut(nameof(UpdateOrder))]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            var response = await _orderServices.UpdateOrder(request);
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpDelete(nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderServices.DeleteOrder(id);
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpOptions]
        public IActionResult CheckIfStatusCodeIsNotFound(HttpStatusCode code, OrderResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
