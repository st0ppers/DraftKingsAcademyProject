using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;

namespace Keyboard.BL.Services
{
    public class KeyboardServices : IKeyboardService
    {
        private readonly IKeyboardSqlRepository _repository;

        public KeyboardServices(IKeyboardSqlRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<KeyboardModel>> GetAllKeyboards()
        {
            return await _repository.GetAllKeyboards();
        }

        public async Task<KeyboardModel> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<KeyboardModel> CreateKeyboard(KeyboardModel keyboard)
        {
            return await _repository.CreateKeyboard(keyboard);
        }

        public async Task<KeyboardModel> UpdateKeyboard(KeyboardModel keyboard)
        {
            return await _repository.UpdateKeyboard(keyboard);
        }

        public async Task<KeyboardModel> DeleteKeyboard(int id)
        {
            return await _repository.DeleteKeyboard(id);
        }
    }
}
