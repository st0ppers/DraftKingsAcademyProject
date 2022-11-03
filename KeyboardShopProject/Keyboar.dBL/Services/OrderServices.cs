using System.Net;
using AutoMapper;
using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using Microsoft.Extensions.Logging;

namespace Keyboard.BL.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderSqlRepository _orderSqlRepository;
        private readonly ILogger<OrderServices> _logger;
        private readonly IMapper _mapper;

        public OrderServices(IOrderSqlRepository orderSqlRepository, ILogger<OrderServices> logger, IMapper mapper)
        {
            _orderSqlRepository = orderSqlRepository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            return await _orderSqlRepository.GetAllOrders();
        }

        public async Task<OrderModel> GetById(int id)
        {
            return await _orderSqlRepository.GetById(id);
        }

        public async Task<OrderResponse> CreateOrder(AddOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderResponse> UpdateOrder(UpdateOrderRequest request)
        {
            if (await _orderSqlRepository.GetById(request.OrderID) == null)
            {
                return new OrderResponse()
                {
                    Message = "No order with that name exists",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var order = _mapper.Map<OrderModel>(request);
            var result = await _orderSqlRepository.UpdateOrder(order);

            return new OrderResponse()
            {
                Message = $"Successfully updated order with id {result.OrderID}",
                StatusCode = HttpStatusCode.OK,
                Order = result
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
                Order = order
            };
        }
    }
}
