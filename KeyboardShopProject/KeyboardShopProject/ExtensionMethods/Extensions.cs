using KafkaServices.Services;
using Keyboard.BL.Interfaces;
using Keyboard.BL.Services;
using Keyboard.DL.Interfaces;
using Keyboard.DL.Repositorys;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.ExtensionMethods
{
    public static class Extensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IKeyboardSqlRepository, KeyboardSqlRepository>();
            services.AddSingleton<IClientSqlRepository, ClientSqlRepository>();
            services.AddSingleton<IOrderSqlRepository, OrderSqlRepository>();
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IKeyboardService, KeyboardServices>();
            services.AddSingleton<IOrderServices, OrderServices>();
            services.AddSingleton<KafkaKeyboardProducer<int, AddKeyboardRequest>>();
            return services;
        }
    }
}
