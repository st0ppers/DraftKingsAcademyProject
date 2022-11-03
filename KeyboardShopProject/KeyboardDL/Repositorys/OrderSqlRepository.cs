using System.Data.SqlClient;
using Dapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Keyboard.DL.Repositorys
{
    public class OrderSqlRepository : IOrderSqlRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderSqlRepository> _logger;

        public OrderSqlRepository(IConfiguration configuration, ILogger<OrderSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {

            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM [Order]";
                    await conn.OpenAsync();
                    return await conn.QueryAsync<OrderModel>(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }

        public async Task<OrderModel> GetById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Order WITH (NOLOCK) WHERE OrderID=@OrderID";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<OrderModel>(query, new { OrderID = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error from {nameof(GetById)} with message {e.Message}");
                throw;
            }
        }

        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "INSERT INTO Order (KeyboardID,ClientID,TotalPrice,Date) VALUES (@KeyboardID,@ClientID,@TotalPrice,@Date)";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<OrderModel>(query, order);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error from {nameof(CreateOrder)} with message {e.Message}");
                throw;
            }
        }

        public async Task<OrderModel> UpdateOrder(OrderModel order)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "UPDATE Order SET KeyboardID=@KeyboardID,ClientID=@ClientID,TotalPrice=@TotalPrice,Date=@Date WHERE OrderID=@OrderID";
                    conn.Open();
                    await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, order);
                    return await GetById(order.OrderID);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(UpdateOrder)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<OrderModel> DeleteOrder(int id)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "DELETE FROM Order WHERE OrderID=@OrderID";
                    conn.Open();
                    var order = await GetById(id);
                    await conn.QueryFirstOrDefaultAsync<OrderModel>(query, new { OrderID = id });
                    return order;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(DeleteOrder)} with message {e.Message}");
                    throw;
                }
            }
        }
    }
}
