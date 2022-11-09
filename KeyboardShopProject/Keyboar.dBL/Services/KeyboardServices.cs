using System.Net;
using AutoMapper;
using KafkaServices.KafkaSettings;
using KafkaServices.Services.Producer;
using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;
using Microsoft.Extensions.Options;

namespace Keyboard.BL.Services
{
    public class KeyboardServices : IKeyboardService
    {
        private readonly IKeyboardSqlRepository _repository;
        private readonly IMapper _mapper;
        private readonly KafkaKeyboardProducer _kafkaProducer;
        public KeyboardServices(IKeyboardSqlRepository repository, IMapper mapper,IOptionsMonitor<KafkaSettingsForKeyboard> settings)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducer = new KafkaKeyboardProducer(settings);
        }

        public async Task<IEnumerable<KeyboardModel>> GetAllKeyboards()
        {
            return await _repository.GetAllKeyboards();
        }

        public async Task<KeyboardResponse> GetById(int id)
        {
            if (await _repository.GetById(id) == null)
            {
                return new KeyboardResponse()
                {
                    Message = "Keyboard with that id doesn't exist",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var keyboard = await _repository.GetById(id);
            return new KeyboardResponse()
            {
                Keyboard = keyboard,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<KeyboardResponse> GetByModel(string name)
        {
            var keyboard = await _repository.GetByModel(name);
            return new KeyboardResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Keyboard = keyboard
            };
        }
        public async Task<KeyboardResponse> CreateKeyboard(AddKeyboardRequest request)
        {
            if (await _repository.GetByModel(request.Model) != null)
            {
                return new KeyboardResponse()
                {
                    Message = "That keyboard model already exists",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var keyboard = _mapper.Map<KeyboardModel>(request);
            var result = await _repository.CreateKeyboard(keyboard);
            var kafkaReport = new KafkaReportModelForKeyboard()
            {
                Price = result.Price,
                Model = result.Model,
                Quantity = result.Quantity,
                Color = result.Color,
                Size = result.Size
            };
            await _kafkaProducer.Produce(result.KeyboardID, kafkaReport, _kafkaProducer.Settings.CurrentValue.Topic,
                _kafkaProducer.Config);
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
            if (keyboardById == null)
            {
                return new KeyboardResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard with that Id doesn't exist"
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
