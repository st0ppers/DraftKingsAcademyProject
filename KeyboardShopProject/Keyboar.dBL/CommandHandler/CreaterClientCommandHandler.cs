using System.Net;
using AutoMapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using Keyboard.Models.Responses;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class CreaterClientCommandHandler : IRequestHandler<CreateClientCommand, ClientResponse>
    {
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IMapper _mapper;
        public CreaterClientCommandHandler(IClientSqlRepository clientSqlRepository, IMapper mapper)
        {
            _clientSqlRepository = clientSqlRepository;
            _mapper = mapper;
        }

        public async Task<ClientResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            if (await _clientSqlRepository.GetByFullName(request.client.FullName) != null)
            {
                return new ClientResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that name already exists"
                };
            }

            var client = _mapper.Map<ClientModel>(request.client);
            var result = await _clientSqlRepository.CreateClient(client);
            return new ClientResponse()
            {
                StatusCode = HttpStatusCode.Created,
                Message = $"Successfully created client with id{client.ClientID}",
                Client = result
            };
        }
    }
}
