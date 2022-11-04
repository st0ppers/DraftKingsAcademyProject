using System.Net;
using KafkaServices.Services;
using Keyboard.BL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyboardController : Controller
    {
        private readonly IKeyboardService _services;
        private readonly KafkaKeyboardProducer<int, AddKeyboardRequest> _kafkaProducer;

        public KeyboardController(IKeyboardService services, KafkaKeyboardProducer<int, AddKeyboardRequest> kafkaProducer)
        {
            _services = services;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet(nameof(GetAllKeyboards))]
        public async Task<IActionResult> GetAllKeyboards()
        {
            return Ok(await _services.GetAllKeyboards());
        }

        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _services.GetById(id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost(nameof(AddKeyboard))]
        public async Task<IActionResult> AddKeyboard([FromBody] AddKeyboardRequest request)
        {
            await _services.CreateKeyboard(request);
            var keyboard = await _services.GetByModel(request.Model);
            await _kafkaProducer.Produce(keyboard.Keyboard.KeyboardID, request);

            return Ok(keyboard);
        }

        [HttpPut(nameof(UpdateKeyboard))]
        public async Task<IActionResult> UpdateKeyboard([FromBody] UpdateKeyboardRequest keyboard)
        {
            return Ok(await _services.UpdateKeyboard(keyboard));
        }

        [HttpDelete(nameof(DeleteKeyboard))]
        public async Task<IActionResult> DeleteKeyboard(int id)
        {
            return Ok(await _services.DeleteKeyboard(id));
        }

        //public IActionResult CheckIfStatusCodeIsNotFound(HttpStatusCode code,)
        //{
        //    if (code == HttpStatusCode.NotFound)
        //    {
        //        return NotFound();
        //    }

        //    return Ok();
        //}
    }
}
