using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.Models.Commands
{
    public record CreateClientCommand(AddClientRequest client) : IRequest<ClientResponse>
    {
    }
}
