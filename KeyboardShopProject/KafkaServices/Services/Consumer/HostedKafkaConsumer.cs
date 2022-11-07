using KafkaServices.KafkaSettings;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Keyboard.Models.Requests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Consumer
{
    public class HostedKafkaConsumer : IHostedService
    {
        private KafkaOrderConsumer<int, AddOrderRequest> _consumer;
        private readonly IMonthlyReportRepository _monthlyReportRepository;
        private readonly IKeyboardSqlRepository _keyboardSqlRepository;

        public HostedKafkaConsumer(IKeyboardSqlRepository keyboardSqlRepository, IOptionsMonitor<KafkaSettingsForOrder> settings, IMonthlyReportRepository monthlyReportRepository)
        {
            _keyboardSqlRepository = keyboardSqlRepository;
            _monthlyReportRepository = monthlyReportRepository;
            _consumer = new KafkaOrderConsumer<int, AddOrderRequest>(settings, keyboardSqlRepository);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting");
            Task.Run(async () =>
            {
                while (true)
                {
                    var o = await _consumer.Consume();
                    if (o == null) continue;
                    var keyboard = await _keyboardSqlRepository.GetById(o.KeyboardID);
                    var month = new MonthlyReportModel()
                    {
                        MonthlySales = 1,
                        Month = o.Date.ToString("MMM"),
                        TotalIncomeForMonth = keyboard.Price,
                    };
                    await _monthlyReportRepository.UpdateMonthlyReport(month);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("shutting down");
            return Task.CompletedTask;
        }
    }
}
