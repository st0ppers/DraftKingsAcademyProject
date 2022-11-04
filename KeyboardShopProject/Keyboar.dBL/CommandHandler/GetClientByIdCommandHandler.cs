using System.Net;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class GetClientByIdCommandHandler : IRequestHandler<GetClientByIdCommand, ClientResponse>
    {
        private readonly IClientSqlRepository _clientSqlRepository;

        public GetClientByIdCommandHandler(IClientSqlRepository clientSqlRepository)
        {
            _clientSqlRepository = clientSqlRepository;
        }

        public async Task<ClientResponse> Handle(GetClientByIdCommand request, CancellationToken cancellationToken)
        {
            if (await _clientSqlRepository.GetById(request.id) == null)
            {
                return new ClientResponse()
                {
                    Message = "Client with that Id doesn't exist",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var client = await _clientSqlRepository.GetById(request.id);
            return new ClientResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Client = client
            };
        }
    }
}
