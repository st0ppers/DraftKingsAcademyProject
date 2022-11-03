using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Keyboard.ShopProject.CustomHealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken token = new CancellationToken())
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await conn.OpenAsync(token);
                }
                catch (Exception e)
                {
                    return HealthCheckResult.Unhealthy("SQL Connection has problem");
                }
            }
            return HealthCheckResult.Healthy("SQL Connection is OK");
        }
    }
}
