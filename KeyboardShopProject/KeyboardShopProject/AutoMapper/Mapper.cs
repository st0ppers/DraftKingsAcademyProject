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
            CreateMap<AddClientRequest, ClientModel>();
            CreateMap<UpdateClientRequest, ClientModel>();
            CreateMap<AddOrderRequest, OrderModel>();
            CreateMap<UpdateOrderRequest, OrderModel>();
            CreateMap<ShoppingCartModel, OrderModel>();
            CreateMap<OrderModel, KafkaReportModelForOrder>();
            CreateMap<KeyboardModel, KafkaReportModelForKeyboard>();
            CreateMap<ClientModel, KafkaReportModelForClient>();
        }
    }
}
