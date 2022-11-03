using System.Net;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Services
{
    public class KeyboardServices : IKeyboardService
    {
        private readonly IKeyboardSqlRepository _repository;
        private readonly IMapper _mapper;
        public KeyboardServices(IKeyboardSqlRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<KeyboardModel>> GetAllKeyboards()
        {
            return await _repository.GetAllKeyboards();
        }

        public async Task<KeyboardModel> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<KeyboardResponse> CreateKeyboard(AddKeyboardRequest request)
        {
            //if (await _repository.GetByModel(request.Model) != null)
            //{
            //    return new KeyboardResponse()
            //    {
            //        Message = "That keyboard model already exists",
            //        StatusCode = HttpStatusCode.NotFound,

            //    };
            //}

            var keyboard = _mapper.Map<KeyboardModel>(request);
            var result = await _repository.CreateKeyboard(keyboard);

            return new KeyboardResponse()
            {
                Keyboard = result,
                StatusCode = HttpStatusCode.Created,
                Message = $"Successfully added keyboard with Id={result.KeyboardID}"
            };
        }

        public async Task<KeyboardResponse> UpdateKeyboard(UpdateKeyboardRequest request)
        {
            var keyboardById = await _repository.GetById(request.KeyboardID);
            var keyboardByModel = await _repository.GetByModel(request.Model);
            if (keyboardById == null)
            {
                return new KeyboardResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard with that Id doesn't exist"
                };
            }
            if (keyboardByModel.Model == keyboardById.Model)
            {
                return new KeyboardResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard with that model doesn't exist"
                };
            }

            var keyboard = _mapper.Map<KeyboardModel>(request);
            var result = await _repository.UpdateKeyboard(keyboard);

            return new KeyboardResponse()
            {
                Keyboard = result,
                StatusCode = HttpStatusCode.OK,
                Message = $"Successfully updated keyboard with Id={result.KeyboardID}"
            };
        }

        public async Task<KeyboardResponse> DeleteKeyboard(int id)
        {
            if (await _repository.GetById(id) == null)
            {
                return new KeyboardResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard with that Id doesn't exist"
                };
            }
            var keyboard = await _repository.DeleteKeyboard(id);
            return new KeyboardResponse()
            {
                Keyboard = keyboard,
                StatusCode = HttpStatusCode.NoContent,
                Message = $"Successfully deleted keyboard with Id={id}"
            };
        }
    }
}
