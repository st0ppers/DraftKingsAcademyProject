using Keyboard.BL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Services
{
    public class ClientService : IClientService
    {
        public Task<IEnumerable<ClientModel>> GetAllClients()
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse> CreateClient(AddClientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse> UpdateClient(UpdateClientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse> DeleteClient(int id)
        {
            throw new NotImplementedException();
        }
    }
}
