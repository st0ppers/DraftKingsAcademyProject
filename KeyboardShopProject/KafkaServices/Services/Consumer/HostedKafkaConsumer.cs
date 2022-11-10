using KafkaServices.KafkaSettings;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KafkaServices.Services.Consumer
{
    public class HostedKafkaConsumer : IHostedService
    {
        private readonly KafkaOrderConsumer<int, KafkaReportModelForOrder> _consumer;
        private readonly IMonthlyReportRepository _monthlyReportRepository;

        public HostedKafkaConsumer(IOptionsMonitor<KafkaSettingsForOrder> settings, IMonthlyReportRepository monthlyReportRepository)
        {
            _monthlyReportRepository = monthlyReportRepository;
            _consumer = new KafkaOrderConsumer<int, KafkaReportModelForOrder>(settings);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var response = await _consumer.Consume();
                    if (response == null) continue;
                    var month = new MonthlyReportModel()
                    {
                        MonthlySales = 1,
                        Month = response.DateOfOrder.ToString("MMM"),
                        TotalIncomeForMonth = response.TotalPrice,
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
