using Keyboard.Models.Models;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.Models.Commands
{
    public record DeleteClientCommand (int id): IRequest<ClientResponse>
    {
    }
}
