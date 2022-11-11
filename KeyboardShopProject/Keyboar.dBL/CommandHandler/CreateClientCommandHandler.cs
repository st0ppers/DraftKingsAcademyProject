using System.Net;
using AutoMapper;
using KafkaServices.KafkaSettings;
using KafkaServices.Services.Producer;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using Keyboard.Models.Responses;
using MediatR;
using Microsoft.Extensions.Options;

namespace Keyboard.BL.CommandHandler
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientResponse>
    {
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IMapper _mapper;
        private readonly KafkaClientProducer _kafkaProducer;

        public CreateClientCommandHandler(IClientSqlRepository clientSqlRepository, IMapper mapper, IOptionsMonitor<KafkaSettingsForClient> settings)
        {
            _clientSqlRepository = clientSqlRepository;
            _mapper = mapper;
            _kafkaProducer = new KafkaClientProducer(settings);
        }

        public async Task<ClientResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var doesClientExist = await _clientSqlRepository.GetByFullName(request.client.FullName);
            if (doesClientExist != null)
            {
                return new ClientResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that name already exists"
                };
            }

            var client = _mapper.Map<ClientModel>(request.client);
            var result = await _clientSqlRepository.CreateClient(client);
            var kafkaReport = _mapper.Map<KafkaReportModelForClient>(result);
            await _kafkaProducer.Produce(result.ClientID, kafkaReport, _kafkaProducer.Settings.CurrentValue.Topic, _kafkaProducer.Config);
            return new ClientResponse()
            {
                StatusCode = HttpStatusCode.Created,
                Message = $"Successfully created client with id {result.ClientID}",
                Client = result
            };
        }
    }
}
