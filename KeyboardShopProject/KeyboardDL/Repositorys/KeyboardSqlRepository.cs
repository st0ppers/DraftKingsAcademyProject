using System.Data.SqlClient;
using Dapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;


namespace Keyboard.DL.Repositorys
{
    public class KeyboardSqlRepository : IKeyboardSqlRepository
    {
        private readonly IConfiguration _configuration;

        public KeyboardSqlRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<KeyboardModel>> GetAllKeyboards()
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Keyboard WITH (NOLOCK)";
                    conn.Open();
                    return await conn.QueryAsync<KeyboardModel>(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> GetById(int id)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM Keyboard WITH (NOLOCK) WHERE KeyboardID=@KeyboardID";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, new { KeyboardID = id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> CreateKeyboard(KeyboardModel keyboard)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "INSERT INTO Keyboard (Size,Model,Price,Quantity,Color) VALUES (@Size,@Model,@Price,@Quantity,@Color)";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, keyboard);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> UpdateKeyboard(KeyboardModel keyboard)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "UPDATE Keyboard SET Size=@Size,Model=@Model,Price=@Price,Quantity=@Quantity,Color=@Color WHERE  KeyboardID=@KeyboardID ";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, keyboard);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<KeyboardModel> DeleteKeyboard(int id)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "DELETE FROM Keyboard WHERE KeyboardID=@KeyboardID";
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<KeyboardModel>(query, new { KeyboardID = id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}