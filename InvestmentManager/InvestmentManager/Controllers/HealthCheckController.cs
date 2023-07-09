using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InvestmentManager.Controllers
{

    [ApiController]
    [AllowAnonymous]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        }

        /// <summary>
        /// Executes the health check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/health/controller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckHealth()
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

            object healthcheckResult = MapHealthResponse(await _healthCheckService.CheckHealthAsync());
            healthcheckResult = healthcheckResult.ToJToken().ToString(Newtonsoft.Json.Formatting.Indented);

            return Ok(healthcheckResult);
        }

        /// <summary>
        /// Redirects to health/controller endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/healthcheck")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Check()
        {
            return Redirect("./health/controller");
        }

        /// <summary>
        /// Returns an OK status code to indicate the service is reachable.
        /// </summary>
        [HttpGet]
        [Route("~/health/ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return Ok();
        }

        /// <summary>
        /// Returns the corresponding version of the app.
        /// </summary>
        /// <returns>The version result.</returns>
        [HttpGet]
        [Route("~/health/assembly")]
      
        public IActionResult ProductVersion()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            object versionInfo = new
            {
                Application = entryAssembly!.GetName().Name,
                Host = Environment.MachineName,
                QueryDate = DateTime.UtcNow.ToString("u"),
                BuildDate = InvestmentManager.HealthChecks.BuildDateInfo.GetBuildDate(Assembly.GetExecutingAssembly()).ToString("u"),
                ProductVersion = FileVersionInfo.GetVersionInfo(entryAssembly.Location).ProductVersion,
                AssemblyVersion = entryAssembly.GetName().Version
            };

            return Ok(versionInfo.ToJToken().ToString(Newtonsoft.Json.Formatting.Indented));
        }

    }
}
