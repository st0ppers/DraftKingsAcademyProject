using Keyboard.Models.Models;
using MediatR;

namespace Keyboard.Models.Commands
{
    public record GetAllClientsCommand : IRequest<IEnumerable<ClientModel>>
    {
    }
}
