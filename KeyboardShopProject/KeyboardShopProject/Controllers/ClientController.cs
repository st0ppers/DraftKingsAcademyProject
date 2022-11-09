﻿using System.Net;
using Keyboard.Models.Commands;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetAllClients))]
        public async Task<IActionResult> GetAllClients()
        {
            return Ok(await _mediator.Send(new GetAllClientsCommand()));
        }

        [HttpGet(nameof(GetClientById))]
        public async Task<IActionResult> GetClientById(int id)
        {
            var response = await _mediator.Send(new GetClientByIdCommand(id));
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpPost(nameof(CreateClient))]
        public async Task<IActionResult> CreateClient([FromBody] AddClientRequest request)
        {
            var response = await _mediator.Send(new CreateClientCommand(request));
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpPut(nameof(UpdateClient))]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            var response = await _mediator.Send(new UpdateClientCommand(request));
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }

        [HttpDelete(nameof(DeleteClient))]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var response = await _mediator.Send(new DeleteClientCommand(id));
            return CheckIfStatusCodeIsNotFound(response.StatusCode, response);
        }
        [HttpOptions]
        public IActionResult CheckIfStatusCodeIsNotFound(HttpStatusCode code, ClientResponse response)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
