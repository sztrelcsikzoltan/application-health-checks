using InvestmentManager.Services;
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
        private readonly CheckHealthService _checkHealthService;

        public HealthCheckController(HealthCheckService healthCheckService, CheckHealthService checkHealthService)
        {
            _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
            _checkHealthService = checkHealthService ?? throw new ArgumentNullException(nameof(checkHealthService));
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
            return await _checkHealthService.CheckHealth();
        }

        /// <summary>
        /// Redirects to health/controller endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/healthcheck")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Redirect()
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
      
        public IActionResult VersionInfo()
        {
            return _checkHealthService.VersionInfo();
        }

    }
}
