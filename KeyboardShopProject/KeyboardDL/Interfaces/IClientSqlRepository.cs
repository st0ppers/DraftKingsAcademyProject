using Keyboard.Models.Models;
using Keyboard.Models.Requests;

namespace Keyboard.DL.Interfaces
{
    public interface IClientSqlRepository
    {
        public Task<IEnumerable<ClientModel>> GetAllClients();
        public Task<ClientModel> GetById(int id);
        public Task<ClientModel> GetByFullName(string clientName);
        public Task<ClientModel> CreateClient(ClientModel client);
        public Task<ClientModel> UpdateClient(ClientModel client);
        public Task<ClientModel> DeleteClient(int id);
    }
}
