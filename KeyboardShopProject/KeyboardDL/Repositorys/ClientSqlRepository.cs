using System.Data.SqlClient;
using Dapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Keyboard.DL.Repositorys
{
    public class ClientSqlRepository : IClientSqlRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientSqlRepository> _logger;

        public ClientSqlRepository(IConfiguration configuration, ILogger<ClientSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<ClientModel>> GetAllClients()
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Client WITH (NOLOCK)";
                    await conn.OpenAsync();
                    return await conn.QueryAsync<ClientModel>(query);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetAllClients)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<ClientModel> GetById(int id)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Client WITH (NOLOCK) WHERE ClientID=@ClientID";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<ClientModel>(query, new { ClientID = id });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetById)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<ClientModel> GetByFullName(string clientName)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Client WITH (NOLOCK) WHERE FullName=@FullName";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<ClientModel>(query, new { FullName = clientName });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetByFullName)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<ClientModel> CreateClient(ClientModel client)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "INSERT INTO Client OUTPUT Inserted.* VALUES (@FullName,@Address,@Age)";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<ClientModel>(query, client);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(CreateClient)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<ClientModel> UpdateClient(ClientModel client)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "UPDATE Client SET FullName=@FullName,Address=@Address,Age=@Age WHERE ClientID=@ClientID";
                    await conn.OpenAsync();
                    await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, client);
                    return await GetById(client.ClientID);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(UpdateClient)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<ClientModel> DeleteClient(int id)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "DELETE FROM Client OUTPUT Deleted.* WHERE ClientID=@ClientID";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<ClientModel>(query, new { ClientID = id });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(DeleteClient)} with message {e.Message}");
                    throw;
                }
            }
        }
    }
}
