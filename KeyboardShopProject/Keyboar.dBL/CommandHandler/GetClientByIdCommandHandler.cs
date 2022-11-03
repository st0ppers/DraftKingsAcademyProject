using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using MediatR;

namespace Keyboard.BL.CommandHandler
{
    public class GetClientByIdCommandHandler : IRequestHandler<GetClientByIdCommand, ClientModel>
    {
        private readonly IClientSqlRepository _clientSqlRepository;

        public GetClientByIdCommandHandler(IClientSqlRepository clientSqlRepository)
        {
            _clientSqlRepository = clientSqlRepository;
        }

        public async Task<ClientModel> Handle(GetClientByIdCommand request, CancellationToken cancellationToken)
        {
            return await _clientSqlRepository.GetById(request.id);
        }
    }
}
