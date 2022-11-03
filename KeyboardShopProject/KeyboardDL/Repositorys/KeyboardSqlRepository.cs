using System.Data.SqlClient;
using Dapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Keyboard.DL.Repositorys
{
    public class KeyboardSqlRepository : IKeyboardSqlRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KeyboardSqlRepository> _logger;

        public KeyboardSqlRepository(IConfiguration configuration, ILogger<KeyboardSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<KeyboardModel>> GetAllKeyboards()
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Keyboard WITH (NOLOCK)";
                    conn.Open();
                    return await conn.QueryAsync<KeyboardModel>(query);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetAllKeyboards)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> GetById(int id)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Keyboard WITH (NOLOCK) WHERE KeyboardID=@KeyboardID";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, new { KeyboardID = id });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetById)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> GetByModel(string modelName)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Keyboard WITH (NOLOCK) WHERE Model=@Model";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, new { Model = modelName });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetByModel)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> CreateKeyboard(KeyboardModel keyboard)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "INSERT INTO Keyboard (Size,Model,Price,Quantity,Color) VALUES (@Size,@Model,@Price,@Quantity,@Color)";
                    conn.Open();
                    await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, keyboard);
                    return await GetByModel(keyboard.Model);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(CreateKeyboard)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> UpdateKeyboard(KeyboardModel keyboard)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "UPDATE Keyboard SET Size=@Size,Model=@Model,Price=@Price,Quantity=@Quantity,Color=@Color WHERE  KeyboardID=@KeyboardID ";
                    conn.Open();
                    await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, keyboard);
                    return await GetById(keyboard.KeyboardID);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(UpdateKeyboard)} with message {e.Message}");
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> DeleteKeyboard(int id)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "DELETE FROM Keyboard WHERE KeyboardID=@KeyboardID";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, new { KeyboardID = id });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(DeleteKeyboard)} with message {e.Message}");
                    throw;
                }
            }
        }
    }
}