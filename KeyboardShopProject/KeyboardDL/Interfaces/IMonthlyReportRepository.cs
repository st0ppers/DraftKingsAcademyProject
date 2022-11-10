using Keyboard.Models.Models;

namespace Keyboard.DL.Interfaces
{
    public interface IMonthlyReportRepository
    {
        public Task<MonthlyReportModel> GetByMonth();
        public Task<MonthlyReportModel> UpdateMonthlyReport(MonthlyReportModel model);
        public Task<MonthlyReportModel> Insert(MonthlyReportModel model);
        public Task<MonthlyReportModel> DeleteReport();
    }
}
