using AutoMapper;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AddKeyboardRequest, KeyboardModel>();
            CreateMap<UpdateKeyboardRequest, KeyboardModel>();
        }
    }
}
