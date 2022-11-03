using AutoMapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class GetAllClientsCommandHandler : IRequestHandler<GetAllClientsCommand,IEnumerable<ClientModel>>
    {
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IMapper _mapper;

        public GetAllClientsCommandHandler(IClientSqlRepository clientSqlRepository, IMapper mapper)
        {
            _clientSqlRepository = clientSqlRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientModel>> Handle(GetAllClientsCommand request, CancellationToken cancellationToken)
        {
            return await _clientSqlRepository.GetAllClients();
        }
    }
}
