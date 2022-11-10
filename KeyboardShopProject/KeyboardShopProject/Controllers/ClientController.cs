using Keyboard.Models.Commands;
using Keyboard.Models.Requests;
using Keyboard.ShopProject.Support;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CheckForStatusCode _check;

        public ClientController(IMediator mediator, CheckForStatusCode check)
        {
            _mediator = mediator;
            _check = check;
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
            return _check.CheckClientResponse(response.StatusCode,response);
        }

        [HttpPost(nameof(CreateClient))]
        public async Task<IActionResult> CreateClient([FromBody] AddClientRequest request)
        {
            var response = await _mediator.Send(new CreateClientCommand(request));
            return _check.CheckClientResponse(response.StatusCode, response);
        }

        [HttpPut(nameof(UpdateClient))]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest request)
        {
            var response = await _mediator.Send(new UpdateClientCommand(request));
            return _check.CheckClientResponse(response.StatusCode, response);
        }

        [Authorize]
        [HttpDelete(nameof(DeleteClient))]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var response = await _mediator.Send(new DeleteClientCommand(id));
            return _check.CheckClientResponse(response.StatusCode, response);
        }
    }
}
