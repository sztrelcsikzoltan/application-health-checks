using InvestmentManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentManager.HealthChecks
{
    public static class IHealthChecksBuilderExtensions
    {
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
