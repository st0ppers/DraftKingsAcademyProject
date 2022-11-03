using Keyboard.BL.Interfaces;
using Keyboard.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KeyboardShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyboardController : Controller
    {
        private readonly IKeyboardService _services;

        public KeyboardController(IKeyboardService services)
        {
            _services = services;
        }

        [HttpGet(nameof(GetAllKeyboards))]
        public async Task<IActionResult> GetAllKeyboards()
        {
            return Ok(await _services.GetAllKeyboards());
        }

        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _services.GetById(id));
        }

        [HttpPost(nameof(AddKeyboard))]
        public async Task<IActionResult> AddKeyboard([FromBody] AddKeyboardRequest keyboard)
        {
            return Ok(await _services.CreateKeyboard(keyboard));
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
    }
}
