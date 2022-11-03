using Keyboard.Models.Healthchecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Keyboard.ShopProject.HealthChecks
{
    public static class HealthCheckExtension
    {
        public static IApplicationBuilder RegisterHealthCheck(this IApplicationBuilder application)
        {
            return application.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthcheckResponse()
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse()
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Dsecription = x.Value.Description
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, Formatting.Indented));
                }
            });
        }
    }
}
