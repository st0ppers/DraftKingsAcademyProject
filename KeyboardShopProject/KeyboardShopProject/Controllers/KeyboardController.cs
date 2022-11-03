﻿using FluentValidation;
using Keyboard.BL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KeyboardShopProject.Controllers
{
    public class KeyboardController : Controller
    {
        private readonly IKeyboardService _services;
        private readonly IValidator<AddKeyboardRequest> _validator;

        public KeyboardController(IKeyboardService services, IValidator<AddKeyboardRequest> validator)
        {
            _services = services;
            _validator = validator;
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

            var re = await _validator.ValidateAsync(keyboard);
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
