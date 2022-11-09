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
        private readonly IOrderSqlRepository _orderSqlRepository;
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IShoppingCartMongoRepository _shoppingCartMongoRepository;
        private readonly KafkaOrderProducer _kafkaProducer;
        private readonly IMapper _mapper;


        public OrderServices(IOrderSqlRepository orderSqlRepository, IMapper mapper, IKeyboardSqlRepository keyboardSqlRepository, IClientSqlRepository clientSqlRepository, IShoppingCartMongoRepository shoppingCartMongoRepository, IOptionsMonitor<KafkaSettingsForOrder> settings)
        {
            _orderSqlRepository = orderSqlRepository;
            _mapper = mapper;
            _keyboardSqlRepository = keyboardSqlRepository;
            _clientSqlRepository = clientSqlRepository;
            _shoppingCartMongoRepository = shoppingCartMongoRepository;
            _kafkaProducer = new KafkaOrderProducer(settings);
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            return await _orderSqlRepository.GetAllOrders();
        }

        public async Task<OrderResponse> GetById(int id)
        {
            if (await _orderSqlRepository.GetById(id) == null)
            {
                return new OrderResponse()
                {
                    Message = "Order with that id does not exist",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var order = await _orderSqlRepository.GetById(id);
            return new OrderResponse()
            {
                OrderID = order.OrderID,
                DateOfOrder = order.Date,
                TotalPrice = order.TotalPrice,
                StatusCode = HttpStatusCode.OK,
                Keyboard = new List<KeyboardModel>()
                {
                  await _keyboardSqlRepository.GetById(order.KeyboardID)
                }
            };
        }

        public async Task<OrderResponse> CreateOrder(AddOrderRequest request)
        {
            var order = _mapper.Map<OrderModel>(request);
            var shoppingCart = await _shoppingCartMongoRepository.GetContent(order.ClientID);
            var client = await _clientSqlRepository.GetById(request.ClientID);
            if (client == null)
            {
                return new OrderResponse()
                {
                    Message = "Client with that Id doesn't exist",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
            if (shoppingCart == null)
            {
                return new OrderResponse()
                {
                    Message = "Cannot create order with empty shopping cart",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            foreach (var k in shoppingCart.Keyboards)
            {
                order.TotalPrice += k.Price;
            }
            var keyboards = new List<KeyboardModel>();
            order.ShoppingCartID = shoppingCart.Id;
            var result = await _orderSqlRepository.CreateOrder(order);
            foreach (var k in shoppingCart.Keyboards)
            {
                keyboards.Add(k);
                await _orderSqlRepository.AddOrderedKeyboards(result, k.KeyboardID);
                await _shoppingCartMongoRepository.RemoveFromShoppingCart(new ShoppingCartRequest()
                {
                    ClientId = order.ClientID,
                    KeyboardId = k.KeyboardID
                });
                var keyboard = await _keyboardSqlRepository.GetById(k.KeyboardID);
                keyboard.Quantity--;
                await _keyboardSqlRepository.UpdateKeyboard(keyboard);
            }

            var kafkaOrder = new KafkaReportModelForOrder()
            {
                Keyboards = keyboards,
                DateOfOrder = order.Date,
                OrderID = result.OrderID,
                TotalPrice = result.TotalPrice
            };
            await _kafkaProducer.Produce(result.OrderID, kafkaOrder, _kafkaProducer.Settings.CurrentValue.Topic, _kafkaProducer.Config);
            return new OrderResponse()
            {
                StatusCode = HttpStatusCode.Created,
                Message = $"Successfully create order with id {result.OrderID}",
                OrderID = result.OrderID,
                DateOfOrder = result.Date,
                TotalPrice = result.TotalPrice,
                Keyboard = keyboards
            };
        }

        public async Task<OrderResponse> UpdateOrder(UpdateOrderRequest request)
        {
            var keyboard = await _keyboardSqlRepository.GetById(request.KeyboardID);
            var client = await _clientSqlRepository.GetById(request.ClientID);
            if (keyboard == null)
            {
                return new OrderResponse()
                {
                    Message = "Keyboard with that Id doesn't exist",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            if (client == null)
            {
                return new OrderResponse()
                {
                    Message = "Client with that Id doesn't exist",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            if (await _orderSqlRepository.GetById(request.OrderID) == null)
            {
                return new OrderResponse()
                {
                    Message = "No order with that name exists",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var order = _mapper.Map<OrderModel>(request);
            order.TotalPrice = keyboard.Price;
            var result = await _orderSqlRepository.UpdateOrder(order);

            return new OrderResponse()
            {
                Message = $"Successfully updated order with id {result.OrderID}",
                StatusCode = HttpStatusCode.OK,
                OrderID = result.OrderID,
                DateOfOrder = result.Date,
                TotalPrice = result.TotalPrice,
                Keyboard = new List<KeyboardModel>()
                {
                    keyboard
                }
            };
        }

        public async Task<OrderResponse> DeleteOrder(int id)
        {
            if (await _orderSqlRepository.GetById(id) == null)
            {
                return new OrderResponse()
                {
                    Message = "No order with that name exists",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var order = await _orderSqlRepository.DeleteOrder(id);
            return new OrderResponse()
            {
                Message = $"Successfully deleted order with id {order.OrderID}",
                StatusCode = HttpStatusCode.NoContent,
                OrderID = order.OrderID,
                DateOfOrder = order.Date,
                TotalPrice = order.TotalPrice,
                Keyboard = new List<KeyboardModel>()
                {
                    await _keyboardSqlRepository.GetById(order.KeyboardID)
                }
            };
        }
    }
}
