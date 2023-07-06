using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InvestmentManager.HealthChecks
{
    internal static class EndPointHealthChecks
    {
        public static void MapEndpointHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.Map("/health/extension", requestDelegate: async context =>
            {
                string CustomHealthStatus(HealthStatus healthStatus)
                {
                    switch (healthStatus)
                    {
                        case HealthStatus.Healthy: return "Success".ToString();
                        case HealthStatus.Unhealthy: return "Failure".ToString();
                        case HealthStatus.Degraded: return "Degraded".ToString();
                        default: return healthStatus.ToString();
                    }
                };

                object MapHealthResponse(HealthReport r) => new
                {
                    OverallStatus = CustomHealthStatus(r.Status),
                    TotalChecksDuration = r.TotalDuration.TotalMilliseconds,
                    HealthChecks = r.Entries.Select(e => new
                    {
                        Name = e.Key,
                        Status = CustomHealthStatus(e.Value.Status),
                        Duration = e.Value.Duration.TotalMilliseconds,
                        Description = e.Value.Description!,
                        Exception = e.Value.Exception?.Message.ToString(),
                        Data = e.Value.Data.Select(dicData => new object[] { dicData.Key, dicData.Value })
                    })
                };

                HealthReport? healthReport = await endpoints.ServiceProvider.GetService<HealthCheckService>()!.CheckHealthAsync();
                
                string? healthcheckResult = MapHealthResponse(healthReport)
                    .ToJToken().ToString(Newtonsoft.Json.Formatting.Indented);

                await context.Response.WriteAsync(healthcheckResult);
            });

            endpoints.Map("/healthcheck", requestDelegate: ctx =>
            {
                ctx.Response.Redirect("./health/extension");
                return Task.CompletedTask;
            });

            endpoints.Map("/health/ping", context => { return Task.CompletedTask; });

            endpoints.Map(pattern: "/health/version", requestDelegate: async context =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                
                object versionInfo = new
                {
                    Application = entryAssembly!.GetName().Name,
                    Host = Environment.MachineName,
                    Date = DateTime.UtcNow.ToString("u"),
                    ProductVersion = FileVersionInfo.GetVersionInfo(entryAssembly.Location).ProductVersion,
                    AssemblyVersion = entryAssembly.GetName().Version
                };

                await context.Response.WriteAsJsonAsync(versionInfo
                    .ToJToken().ToString(Newtonsoft.Json.Formatting.Indented));
            });
        }

    }
}
