using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using InvestmentManager.DataAccess.EF;

namespace InvestmentManager.HealthChecks
{
    public class SqlServerHealthCheckThroughDbContext : IHealthCheck
    {

        private readonly InvestmentContext _investmentContext;
        public SqlServerHealthCheckThroughDbContext(InvestmentContext investmentContext)
        {
            _investmentContext = investmentContext;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _investmentContext.Database.SetCommandTimeout(1);
            var dbQuery = _investmentContext.Database.ExecuteSqlRaw("Select Count(1)");

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }

}
