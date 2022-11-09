using System.Net;
using AutoMapper;
using KafkaServices.KafkaSettings;
using Keyboard.BL.CommandHandler;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Commands;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Microsoft.Extensions.Options;
using Moq;
using Mapper = Keyboard.ShopProject.AutoMapper.Mapper;

namespace Keyboard.Tests
{
    public class ClientTest
    {
        private IList<ClientModel> _clients = new List<ClientModel>()
        {
            new()
            {
                Address = "adres 1",
                Age = 12,
                ClientID = 1,
                FullName = "name"
            },
            new()
            {
                Address = "adres 2",
                Age = 1234,
                ClientID = 2,
                FullName = "name 2"
            }
        };

        private readonly IMapper _mapper;
        private readonly Mock<IClientSqlRepository> _mockRepo;
        private  Mock<IOptionsMonitor<KafkaSettingsForClient>> _mockSettings;

        public ClientTest()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<Mapper>();
            });
            _mockRepo = new Mock<IClientSqlRepository>();
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllClientsCountCheck()
        {
            //setup
            _mockRepo.Setup(r => r.GetAllClients()).ReturnsAsync(_clients);
            var excpectedCount = _mockRepo.Object.GetAllClients().Result.Count();

            //inject 
            var handler = new GetAllClientsCommandHandler(_mockRepo.Object, _mapper);

            //Act
            var result = await handler.Handle(new GetAllClientsCommand(), new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), excpectedCount);
        }

        [Fact]
        public async Task GetByIdClientsCheck()
        {
            //setup
            var clientID = 1;
            var clientResult = _clients.FirstOrDefault(x => x.ClientID == clientID);
            _mockRepo.Setup(r => r.GetById(clientID)).ReturnsAsync(clientResult);
            //inject

            var handler = new GetClientByIdCommandHandler(_mockRepo.Object);

            //act
            var result = await handler.Handle(new GetClientByIdCommand(clientID), new CancellationToken());

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Client);
            Assert.Equal(clientID, result.Client.ClientID);
            Assert.Equal(clientResult, result.Client);
        }

        [Fact]
        public async Task GetByIClientBadPath()
        {
            //setup
            var clientID = 10;
            var clientResult = _clients.FirstOrDefault(x => x.ClientID == clientID);
            _mockRepo.Setup(r => r.GetById(clientID)).ReturnsAsync(clientResult);

            //inject
            var handler = new GetClientByIdCommandHandler(_mockRepo.Object);

            //act
            var result = await handler.Handle(new GetClientByIdCommand(clientID), new CancellationToken());

            //assert
            Assert.Null(result.Client);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddClientCheck()
        {
            //setup
            var clientID = 3;
            var client = new AddClientRequest()
            {
                Address = "addres 3",
                Age = 24,
                FullName = "name 3"
            };
            _mockRepo.Setup(r => r.CreateClient(It.IsAny<ClientModel>()))
                .Callback(() => _clients.Add(new ClientModel()
                {
                    Address = client.Address,
                    Age = client.Age,
                    FullName = client.FullName,
                    ClientID = clientID
                }))
                !.ReturnsAsync(() => _clients.FirstOrDefault(x => x.ClientID == clientID));

            _mockRepo.Setup(x => x.GetByFullName(client.FullName))
                .ReturnsAsync(_clients.FirstOrDefault(x => x.FullName == client.FullName));
            var settings = new KafkaSettingsForClient()
            {
                AutoOffsetReset = 1,
                BootstrapServers = "localhost:9092",
                Topic = "Client",
                GroupId = "ClientGroup1"
            };
            _mockSettings = new Mock<IOptionsMonitor<KafkaSettingsForClient>>();
            _mockSettings.Setup(s => s.CurrentValue).Returns(settings);

            //inject
            var handler = new CreateClientCommandHandler(_mockRepo.Object, _mapper, _mockSettings.Object);

            //act
            var result = await handler.Handle(new CreateClientCommand(client), new CancellationToken());

            //assert 
            Assert.NotNull(result.Client);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(3, _clients.Count);
        }

        [Fact]
        public async Task AddClientCheckBadPath()
        {
            //setup
            var clientId = 3;
            var clientRequest = new AddClientRequest()
            {
                Address = "adres 1",
                Age = 12,
                FullName = "name"
            };
            _mockRepo.Setup(r => r.GetByFullName(clientRequest.FullName))
                .ReturnsAsync(_clients.FirstOrDefault(x => x.FullName == clientRequest.FullName));

            _mockRepo.Setup(r => r.CreateClient(It.IsAny<ClientModel>()))
                .ReturnsAsync(() => _clients.FirstOrDefault(x => x.FullName == clientRequest.FullName));

            var settings = new KafkaSettingsForClient()
            {
                AutoOffsetReset = 1,
                BootstrapServers = "localhost:9092",
                Topic = "Client",
                GroupId = "ClientGroup1"
            };
            _mockSettings = new Mock<IOptionsMonitor<KafkaSettingsForClient>>();
            _mockSettings.Setup(s => s.CurrentValue).Returns(settings);

            //inject
            var handler = new CreateClientCommandHandler(_mockRepo.Object, _mapper, _mockSettings.Object);

            //act
            var result = await handler.Handle(new CreateClientCommand(clientRequest), new CancellationToken());

            //assert 
            Assert.Null(result.Client);
            Assert.Equal(2, _clients.Count);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task UpdateClientCheck()
        {
            //setup
            var clientId = 2;
            var clientTOUpdate = new UpdateClientRequest()
            {
                Address = "updated address",
                Age = 212,
                FullName = "updated name",
                ClientID = clientId
            };
            _mockRepo.Setup(r => r.GetById(clientId))
                .ReturnsAsync(_clients.FirstOrDefault(c => c.ClientID == clientId));

            _mockRepo.Setup(r => r.UpdateClient(It.IsAny<ClientModel>()))
                .ReturnsAsync(() => _clients.FirstOrDefault(x => x.ClientID == clientId));

            //inject
            var handler = new UpdateClientCommandHandler(_mockRepo.Object, _mapper);

            //act
            var result = await handler.Handle(new UpdateClientCommand(clientTOUpdate), new CancellationToken());

            //assert 
            Assert.Equal(_clients.FirstOrDefault(x => x.ClientID == clientId), result.Client);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, _clients.Count);
        }

        [Fact]
        public async Task UpdateClientCheckBadPath()
        {
            //setup
            var clientId = 21;
            var clientTOUpdate = new UpdateClientRequest()
            {
                Address = "updated address",
                Age = 212,
                FullName = "updated name",
                ClientID = clientId
            };
            _mockRepo.Setup(r => r.GetById(clientId))
                .ReturnsAsync(_clients.FirstOrDefault(c => c.ClientID == clientId));

            _mockRepo.Setup(r => r.UpdateClient(It.IsAny<ClientModel>()))
                .ReturnsAsync(() => _clients.FirstOrDefault(x => x.ClientID == clientId));

            //inject
            var handler = new UpdateClientCommandHandler(_mockRepo.Object, _mapper);

            //act
            var result = await handler.Handle(new UpdateClientCommand(clientTOUpdate), new CancellationToken());

            //assert 
            Assert.Null(result.Client);
            Assert.Equal(result.StatusCode, HttpStatusCode.NotFound);
            Assert.Equal(result.Message, "Client with that name doesn't exist");
            Assert.Equal(_clients.Count, 2);
        }

        [Fact]
        public async Task DeleteClientCheck()
        {
            //setup
            var clientId = 2;
            var clientToDelete = _clients.FirstOrDefault(x => x.ClientID == clientId);

            _mockRepo.Setup(r => r.GetById(clientToDelete.ClientID))
                !.ReturnsAsync(_clients.FirstOrDefault(c => c.ClientID == clientToDelete.ClientID));

            _mockRepo.Setup(r => r.DeleteClient(clientToDelete.ClientID))
                .Callback(() =>
                {
                    _clients.Remove(clientToDelete);
                })!.ReturnsAsync(clientToDelete);
            //inject
            var handler = new DeleteClientCommandHandler(_mockRepo.Object);

            //act
            var result = await handler.Handle(new DeleteClientCommand(clientId), new CancellationToken());

            //assert 
            Assert.Equal(1, _clients.Count);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeleteClientCheckBadPath()
        {
            //setup
            var clientId = 12;
            var clientToDelete = _clients.FirstOrDefault(x => x.ClientID == clientId);

            _mockRepo.Setup(r => r.GetById(clientId))
                !.ReturnsAsync(_clients.FirstOrDefault(c => c.ClientID == clientId));

            _mockRepo.Setup(r => r.DeleteClient(clientId))
                .Callback(() =>
                {
                    _clients.Remove(clientToDelete);
                })!.ReturnsAsync(clientToDelete);
            //inject
            var handler = new DeleteClientCommandHandler(_mockRepo.Object);

            //act
            var result = await handler.Handle(new DeleteClientCommand(clientId), new CancellationToken());

            //assert 
            Assert.Equal(2, _clients.Count);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
