using Keyboard.Models.Models;
using MediatR;

namespace Keyboard.Models.Commands
{
    public record GetClientByIdCommand(int id) : IRequest<ClientModel>
    {
    }
}
