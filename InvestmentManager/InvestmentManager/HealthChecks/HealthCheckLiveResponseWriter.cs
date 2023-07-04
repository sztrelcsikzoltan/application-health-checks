using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace InvestmentManager.HealthChecks
{
    public class HealthCheckLiveResponseWriter
    {
        public static Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                    new JProperty("Overall status", result.Status.ToString()),
                    new JProperty("totalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.000000"))
                );
            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }
}
