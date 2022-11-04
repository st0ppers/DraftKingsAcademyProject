using System.Net;
using Keyboard.Models.Commands;
using Keyboard.Models.Requests;
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
            return Ok(await _mediator.Send(new GetClientByIdCommand(id)));
        }

        [HttpPost(nameof(CreateClient))]
        public async Task<IActionResult> CreateClient([FromBody] AddClientRequest request)
        {
            var result = await _mediator.Send(new CreateClientCommand(request));
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return Ok(result.Client);
        }

        [HttpPut(nameof(UpdateClient))]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            var result = await _mediator.Send(new UpdateClientCommand(request));
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return Ok(result.Client);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _mediator.Send(new DeleteClientCommand(id));
            return Ok(result);
        }
    }
}
