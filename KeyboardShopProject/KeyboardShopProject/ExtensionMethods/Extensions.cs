using KafkaServices.KafkaSettings;
using KafkaServices.Services.Consumer;
using KafkaServices.Services.Producer;
using Keyboard.BL.Interfaces;
using Keyboard.BL.Services;
using Keyboard.DL.Interfaces;
using Keyboard.DL.Repositorys;
using Keyboard.Models.Models;
using Keyboard.ShopProject.Support;

namespace Keyboard.ShopProject.ExtensionMethods
{
    public static class Extensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IKeyboardSqlRepository, KeyboardSqlRepository>();
            services.AddSingleton<IClientSqlRepository, ClientSqlRepository>();
            services.AddSingleton<IMonthlyReportRepository, MonthlyReportRepository>();
            services.AddSingleton<IShoppingCartMongoRepository, ShoppingCartMongoRepository>();
            services.AddSingleton<IOrderMongoRepository, OrderMongoRepository>();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IKeyboardService, KeyboardServices>();
            services.AddSingleton<IOrderServices, OrderServices>();
            services.AddSingleton<BaseKafkaProducer<int, KafkaReportModelForClient>>();
            services.AddSingleton<BaseKafkaProducer<int, KafkaReportModelForKeyboard>>();
            services.AddSingleton<BaseKafkaProducer<Guid, KafkaReportModelForOrder>>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<IShoppingCartMongoRepository, ShoppingCartMongoRepository>();
            services.AddSingleton<CheckForStatusCode>();
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
            builder.Services.Configure<MongoConfiguration>(builder.Configuration.GetSection(nameof(MongoConfiguration)));
            return service;
        }
    }
}
