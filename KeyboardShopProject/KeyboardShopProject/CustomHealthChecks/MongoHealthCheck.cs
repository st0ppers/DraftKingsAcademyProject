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

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                _mongoClient = new MongoClient(_settings.CurrentValue.ConnecionString);
                var database = _mongoClient.GetDatabase(_settings.CurrentValue.DatabaseName);
                var collection = database.GetCollection<ShoppingCartModel>(_settings.CurrentValue.CollectionName);
            }
            catch (Exception e)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Problem with Mongo db"));
            }
            return Task.FromResult(HealthCheckResult.Healthy("Mongo connection is healthy"));
        }
    }
}
