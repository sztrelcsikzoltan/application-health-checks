using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace InvestmentManager.HealthChecks
{
    public static class IHealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddSqlServerCheckThroughSqlCommand(this IHealthChecksBuilder builder, string name, IEnumerable<string>? tags = default, TimeSpan? timeout = default)
        {
            builder.Services.AddScoped<SqlServerHealthCheckThroughSqlCommand, SqlServerHealthCheckThroughSqlCommand>();
            builder.AddCheck<SqlServerHealthCheckThroughSqlCommand>(name, tags: tags);
            return builder;
        }

        public static IHealthChecksBuilder AddSqlServerCheckThroughDbContext(this IHealthChecksBuilder builder, string name, IEnumerable<string>? tags = default, TimeSpan? timeout = default)
        {
            builder.Services.AddScoped<SqlServerHealthCheckThroughDbContext, SqlServerHealthCheckThroughDbContext>();
            builder.AddCheck<SqlServerHealthCheckThroughDbContext>(name, tags: tags);
            return builder;
        }

        public static IHealthChecksBuilder AddFilePathWrite(this IHealthChecksBuilder builder, string filePath, HealthStatus failureStatus, IEnumerable<string>? tags = default)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return builder.Add(new HealthCheckRegistration(
                "File Path Health Check extension",
                new FilePathWriteHealthCheck(filePath),
                failureStatus,
                tags));
        }

    }
}
