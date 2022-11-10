using System.Net;
using AutoMapper;
using KafkaServices.KafkaSettings;
using KafkaServices.Services.Producer;
using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using Microsoft.Extensions.Options;

namespace Keyboard.BL.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderMongoRepository _repository;
        private readonly IShoppingCartMongoRepository _shoppingCartMongoRepository;
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;
        private readonly KafkaOrderProducer _producer;
        private readonly IMapper _mapper;

        public OrderServices(IOrderMongoRepository repository, IMapper mapper, IShoppingCartMongoRepository shoppingCartMongoRepository,
            IOptionsMonitor<KafkaSettingsForOrder> settings, IClientSqlRepository clientSqlRepository, IKeyboardSqlRepository keyboardSqlRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _shoppingCartMongoRepository = shoppingCartMongoRepository;
            _clientSqlRepository = clientSqlRepository;
            _keyboardSqlRepository = keyboardSqlRepository;
            _producer = new KafkaOrderProducer(settings);
        }

        public async Task<OrderResponse> GetById(Guid id)
        {
            var order = await _repository.GetOrder(id);
            foreach (var k in order.Keyboards)
            {
                order.TotalPrice += k.Price;
            }
            return new OrderResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Order = order
            };
        }

        public async Task<OrderResponse> CreateOrder(int clientId)
        {
            var shoppingCart = await _shoppingCartMongoRepository.GetContent(clientId);
            var order = _mapper.Map<OrderModel>(shoppingCart);
            order.Date=DateTime.Now;
            if (shoppingCart == null)
            {
                return new OrderResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Your shopping cart is empty"
                };
            }
            if (await _clientSqlRepository.GetById(shoppingCart.ClientId) == null)
            {
                return new OrderResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that id doesn't exist"
                };
            }
            foreach (var k in order.Keyboards)
            {
                if (await _keyboardSqlRepository.GetById(k.KeyboardID) == null)
                {
                    return new OrderResponse()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Keyboard with that id doesn't exist"
                    };
                }
            }

            var report = _mapper.Map<KafkaReportModelForOrder>(order);
            await _repository.CreateOrder(order);
            _producer.Produce(report.OrderID, report, _producer.Settings.CurrentValue.Topic, _producer.Config);
            await _shoppingCartMongoRepository.EmptyShoppingCart(clientId);
            return new OrderResponse()
            {
                Order = order,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<OrderResponse> UpdateOrder(UpdateOrderRequest request)
        {
            var order = _mapper.Map<OrderModel>(request);
            if (await GetById(request.OrderID) == null)
            {
                return new OrderResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Order with that id doesn't exist"
                };
            }
            await _repository.UpdateOrder(order);
            foreach (var k in order.Keyboards)
            {
                order.TotalPrice += k.Price;
            }
            return new OrderResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Order = order
            };
        }

        public async Task<OrderResponse> DeleteOrder(Guid id)
        {
            var order = await _repository.DeleteOrder(id);
            return new OrderResponse()
            {
                StatusCode = HttpStatusCode.NoContent,
                Order = order
            };
        }
    }
}
