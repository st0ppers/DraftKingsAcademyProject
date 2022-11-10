using System.Net;
using Keyboard.BL.Interfaces;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartMongoRepository _shoppingCartRepository;
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;
        private readonly IClientSqlRepository _clientSqlRepository;

        public ShoppingCartService(IShoppingCartMongoRepository repository, IKeyboardSqlRepository keyboardSqlRepository, IClientSqlRepository clientSqlRepository)
        {
            _shoppingCartRepository = repository;
            _keyboardSqlRepository = keyboardSqlRepository;
            _clientSqlRepository = clientSqlRepository;
        }

        public async Task<ShoppingCartResponse> GetContent(int clientID)
        {
            var cart = await _shoppingCartRepository.GetContent(clientID);
            if (cart == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that id doesn't have a shopping cart"
                };
            }

            return new ShoppingCartResponse()
            {
                StatusCode = HttpStatusCode.OK,
                ShoppingCart = cart
            };
        }

        public async Task<ShoppingCartResponse> AddToShoppingCard(ShoppingCartRequest request)
        {
            var response = await GetContent(request.ClientId);
            var keyboardToAdd = await _keyboardSqlRepository.GetById(request.KeyboardId);
            var client = await _clientSqlRepository.GetById(request.ClientId);
            if (response.ShoppingCart == null)
            {
                response.ShoppingCart = await _shoppingCartRepository.CreateShoppingCart(
                    new ShoppingCartModel
                    {
                        ClientId = request.ClientId,
                        Keyboards = new List<KeyboardModel>()
                    });
            }
            if (client == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client with that id doesn't exist",
                    ShoppingCart = response.ShoppingCart
                };
            }
            if (keyboardToAdd == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard with that id doesn't exist",
                    ShoppingCart = response.ShoppingCart
                };
            }
            if (keyboardToAdd.Quantity >= 1)
            {
                response.ShoppingCart = await _shoppingCartRepository.AddToShoppingCard(request);
                keyboardToAdd.Quantity--;
                await _keyboardSqlRepository.UpdateKeyboard(keyboardToAdd);
            }
            else
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "This keyboard is out of stock",
                    ShoppingCart = response.ShoppingCart
                };
            }

            return new ShoppingCartResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"Successfully ordered keyboard {keyboardToAdd.Model}",
                ShoppingCart = response.ShoppingCart
            };
        }

        public async Task<ShoppingCartResponse> RemoveFromShoppingCart(ShoppingCartRequest request)
        {
            var keyboardToRemove = await _keyboardSqlRepository.GetById(request.KeyboardId);
            var client = await _clientSqlRepository.GetById(request.ClientId);
            if (client == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client doesn't exist"
                };
            }

            if (keyboardToRemove == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Keyboard  doesn't exist"
                };
            }
            var response = await GetContent(request.ClientId);
            if (response == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client doesn't have a shopping cart",
                    ShoppingCart = response.ShoppingCart
                };
            }

            if (response.ShoppingCart.Keyboards.FirstOrDefault(x => x.KeyboardID == request.KeyboardId) == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Client doesn't have {keyboardToRemove.Model} in his shopping cart",
                    ShoppingCart = response.ShoppingCart
                };
            }

            var result = await _shoppingCartRepository.RemoveFromShoppingCart(request);
            return new ShoppingCartResponse()
            {
                StatusCode = HttpStatusCode.OK,
                ShoppingCart = result
            };

        }

        public async Task<ShoppingCartResponse> EmptyShoppingCart(int clientID)
        {
            if (await _clientSqlRepository.GetById(clientID) == null)
            {
                return new ShoppingCartResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Client's doesn't exist"
                };
            }

            await _shoppingCartRepository.EmptyShoppingCart(clientID);
            return new ShoppingCartResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Successfully emptied your shopping cart"
            };
        }
    }
}
