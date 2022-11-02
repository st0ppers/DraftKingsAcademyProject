using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IKeyboardSqlRepository
    {
        public Task<IEnumerable<KeyboardModel>> GetAllKeyboards();
        public Task<KeyboardModel> GetById(int id);
        public Task<KeyboardModel> CreateKeyboard(KeyboardModel keyboard);
        public Task<KeyboardModel> UpdateKeyboard(KeyboardModel keyboard);
        public Task<KeyboardModel> DeleteKeyboard(int id);
    }
}
