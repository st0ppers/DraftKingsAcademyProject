using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;

namespace Keyboard.DL.Repositorys
{
    public class ClientSqlRepository : IClientSqlRepository
    {
        private readonly IConfiguration _configuration;
        public Task<IEnumerable<ClientModel>> GetAllClients()
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> GetByFullName(string clientName)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> CreateClient(ClientModel client)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> UpdateClient(ClientModel client)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> DeleteClient(int id)
        {
            throw new NotImplementedException();
        }
    }
}
