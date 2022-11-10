using System.Data.SqlClient;
using Dapper;
using Keyboard.DL.Interfaces;
using Keyboard.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Keyboard.DL.Repositorys
{
    public class MonthlyReportRepository : IMonthlyReportRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MonthlyReportRepository> _logger;

        public MonthlyReportRepository(IConfiguration configuration, ILogger<MonthlyReportRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<MonthlyReportModel> GetByMonth()
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var query = "SELECT * FROM MonthlyReport";
                    await conn.OpenAsync();
                    var result = await conn.QueryAsync<MonthlyReportModel>(query);
                    return result.First();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(GetByMonth)} with error {e.Message}");
                    throw;
                }
            }
        }

        public async Task<MonthlyReportModel> Insert(MonthlyReportModel model)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var addQuery = "INSERT INTO MonthlyReport VALUES (@Month,@MonthlySales,@TotalIncomeForMonth)";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync(addQuery, model);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(Insert)} with error {e.Message}");
                    throw;
                }
            }
        }

        public async Task<MonthlyReportModel> UpdateMonthlyReport(MonthlyReportModel model)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var data = GetByMonth();
                    if (model.Month != data.Result.Month)
                    {
                        await DeleteReport();
                        return await Insert(model);
                    }
                    var reportInDatabase = await GetByMonth();
                    if (reportInDatabase != null)
                    {
                        model.MonthlySales += reportInDatabase.MonthlySales;
                        model.TotalIncomeForMonth += reportInDatabase.TotalIncomeForMonth;
                    }
                    var query = "UPDATE MonthlyReport SET MonthlySales=@MonthlySales,TotalIncomeForMonth=@TotalIncomeForMonth WHERE Month=@Month";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<MonthlyReportModel>(query, model);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(UpdateMonthlyReport)} with error {e.Message}");
                    throw;
                }
            }
        }

        public async Task<MonthlyReportModel> DeleteReport()
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var deleteQuery = "DELETE  FROM MonthlyReport";
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<MonthlyReportModel>(deleteQuery);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error from {nameof(DeleteReport)} with error {e.Message}");
                    throw;
                }
            }
        }
    }
}
