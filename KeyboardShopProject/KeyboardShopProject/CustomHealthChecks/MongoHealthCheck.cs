using Keyboard.Models.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Keyboard.ShopProject.CustomHealthChecks
{
    public class MongoHealthCheck : IHealthCheck
    {
        private MongoClient _mongoClient;
        private readonly IOptionsMonitor<MongoConfiguration> _settings;

        public MongoHealthCheck(IOptionsMonitor<MongoConfiguration> settings)
        {
            _settings = settings;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                _mongoClient = new MongoClient(_settings.CurrentValue.ConnecionString);
                var database = _mongoClient.GetDatabase(_settings.CurrentValue.DatabaseName);
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("Problem with Mongo db");
            }

            return HealthCheckResult.Healthy("Mongo connection is healthy");
        }
    }
}
