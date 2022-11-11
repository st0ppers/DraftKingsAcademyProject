using System.Net;
using AutoMapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientResponse>
    {
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IClientSqlRepository clientSqlRepository, IMapper mapper)
        {
            _clientSqlRepository = clientSqlRepository;
            _mapper = mapper;
        }

        public async Task<ClientResponse> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            if (await _clientSqlRepository.GetById(request.client.ClientID) == null)
            {
                return new ClientResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that name doesn't exist"
                };
            }

            var client = _mapper.Map<ClientModel>(request.client);
            var result = await _clientSqlRepository.UpdateClient(client);

            return new ClientResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Client = result,
                Message = $"Successfully updated client with id {result.ClientID}"
            };
        }
    }
}
