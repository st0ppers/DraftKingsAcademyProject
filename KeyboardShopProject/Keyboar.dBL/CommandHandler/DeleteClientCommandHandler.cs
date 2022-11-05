using System.Net;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, ClientResponse>
    {
        private readonly IClientSqlRepository _clientSqlRepository;

        public DeleteClientCommandHandler(IClientSqlRepository clientSqlRepository)
        {
            _clientSqlRepository = clientSqlRepository;
        }

        public async Task<ClientResponse> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {

            if (await _clientSqlRepository.GetById(request.id) == null)
            {
                return new ClientResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that id doesn't exist",
                };
            }

            var client = await _clientSqlRepository.DeleteClient(request.id);
            return new ClientResponse()
            {
                StatusCode = HttpStatusCode.NoContent,
                Message = $"Successfully delete Client with id {client.ClientID}",
                Client = client,
            };
        }
    }
}
