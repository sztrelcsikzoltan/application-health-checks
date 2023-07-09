using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace InvestmentManager.HealthChecks
{
    // SqlServerHealthCheck
    public class SqlServerHealthCheckThroughSqlCommand : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlServerHealthCheckThroughSqlCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            string connectionString = _configuration.GetConnectionString("InvestmentDatabase");
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand() { Connection = connection, CommandText = "SELECT 1", CommandTimeout = 1 };
            await connection.OpenAsync(cancellationToken);
            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
    }
}
