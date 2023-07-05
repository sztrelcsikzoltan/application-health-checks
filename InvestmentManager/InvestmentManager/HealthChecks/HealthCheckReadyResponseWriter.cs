using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentManager.HealthChecks
{
    public class HealthCheckReadyResponseWriter
    {
        public static Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                    new JProperty("Overall status", result.Status.ToString()),
                    new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.000000")),
                    new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
                        new JProperty(dicItem.Key, new JObject(
                            new JProperty("Status", dicItem.Value.Status.ToString()),
                            new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.000000")),
                           new JProperty("Description", dicItem.Value.Description != null ? dicItem.Value.Description!.ToString() : null),
                            new JProperty("Exception", dicItem.Value.Exception?.Message),
                            new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData => new JProperty(dicData.Key, dicData.Value))))
                        ))
                    )))
            );
            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }
}
