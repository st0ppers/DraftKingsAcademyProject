using KafkaServices.KafkaSettings;
using KafkaServices.Services.Consumer;
using KafkaServices.Services.Producer;
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
            services.AddSingleton<IMonthlyReportRepository, MonthlyReportRepository>();
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IKeyboardService, KeyboardServices>();
            services.AddSingleton<IOrderServices, OrderServices>();
            services.AddSingleton<BaseKafkaProducer<int, AddClientRequest>>();
            services.AddSingleton<BaseKafkaProducer<int, AddKeyboardRequest>>();
            services.AddSingleton<BaseKafkaProducer<int, AddOrderRequest>>();
            return services;
        }

        public static IServiceCollection RegisterIHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<HostedKafkaConsumer>();
            return services;
        }

        public static IServiceCollection RegisterIOptionsMonitor(this IServiceCollection service, WebApplicationBuilder builder)
        {
            builder.Services.Configure<KafkaSettingsForKeyboard>(builder.Configuration.GetSection(nameof(KafkaSettingsForKeyboard)));
            builder.Services.Configure<KafkaSettingsForClient>(builder.Configuration.GetSection(nameof(KafkaSettingsForClient)));
            builder.Services.Configure<KafkaSettingsForOrder>(builder.Configuration.GetSection(nameof(KafkaSettingsForOrder)));
            return service;
        }
    }
}
