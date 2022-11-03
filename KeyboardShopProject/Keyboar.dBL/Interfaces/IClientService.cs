using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<ClientModel>> GetAllClients();
        public Task<ClientModel> GetById(int id);
        public Task<ClientResponse> CreateClient(AddClientRequest request);
        public Task<ClientResponse> UpdateClient(UpdateClientRequest request);
        public Task<ClientResponse> DeleteClient(int id);
    }
}
