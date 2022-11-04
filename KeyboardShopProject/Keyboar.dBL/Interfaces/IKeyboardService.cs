using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Interfaces
{
    public interface IKeyboardService
    {
        public Task<IEnumerable<KeyboardModel>> GetAllKeyboards();
        public Task<KeyboardResponse> GetById(int id);
        public Task<KeyboardResponse> GetByModel(string name);
        public Task<KeyboardResponse> CreateKeyboard(AddKeyboardRequest keyboard);
        public Task<KeyboardResponse> UpdateKeyboard(UpdateKeyboardRequest keyboard);
        public Task<KeyboardResponse> DeleteKeyboard(int id);

    }
}
