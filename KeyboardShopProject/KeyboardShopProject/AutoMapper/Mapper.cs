using AutoMapper;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Keyboard.Models.Responses;

namespace Keyboard.ShopProject.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AddKeyboardRequest, KeyboardModel>();
            CreateMap<UpdateKeyboardRequest, KeyboardModel>();
            CreateMap<AddClientRequest, ClientModel>();
            CreateMap<UpdateClientRequest, ClientModel>();
        }
    }
}
