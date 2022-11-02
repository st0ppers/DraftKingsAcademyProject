using Keyboard.Models.Models;

namespace Keyboard.BL.Interfaces
{
    public interface IKeyboardService
    {
        public Task<IEnumerable<KeyboardModel>> GetAllKeyboards();
        public Task<KeyboardModel> GetById(int id);
        public Task<KeyboardModel> CreateKeyboard(KeyboardModel keyboard);
        public Task<KeyboardModel> UpdateKeyboard(KeyboardModel keyboard);
        public Task<KeyboardModel> DeleteKeyboard(int id);

    }
}
