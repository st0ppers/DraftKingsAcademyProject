using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Keyboard.ShopProject.Support;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyboardController : Controller
    {
        private readonly IKeyboardService _services;
        private readonly CheckForStatusCode _check;

        public KeyboardController(IKeyboardService services, CheckForStatusCode check)
        {
            _services = services;
            _check = check;
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
            return _check.CheckKeyboardResponse(response.StatusCode,response);
        }

        [HttpPost(nameof(AddKeyboard))]
        public async Task<IActionResult> AddKeyboard([FromBody] AddKeyboardRequest request)
        {
            var response = await _services.CreateKeyboard(request);
            return _check.CheckKeyboardResponse(response.StatusCode, response);
        }

        [HttpPut(nameof(UpdateKeyboard))]
        public async Task<IActionResult> UpdateKeyboard([FromBody] UpdateKeyboardRequest request)
        {
            var response = await _services.UpdateKeyboard(request);
            return _check.CheckKeyboardResponse(response.StatusCode, response);
        }

        [HttpDelete(nameof(DeleteKeyboard))]
        public async Task<IActionResult> DeleteKeyboard(int id)
        {
            var response = await _services.DeleteKeyboard(id);
            return _check.CheckKeyboardResponse(response.StatusCode, response);
        }

    }
}
