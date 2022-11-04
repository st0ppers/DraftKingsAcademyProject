using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.Models.Commands
{
    public record GetClientByIdCommand(int id) : IRequest<ClientResponse>
    {
    }
}
