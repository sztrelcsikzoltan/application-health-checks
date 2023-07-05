using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentManager.HealthChecks
{
    public class FilePathWriteHealthCheck : IHealthCheck
    {
        private string _filePath;
        private IReadOnlyDictionary<string, object> _HealthCheckData;

        public FilePathWriteHealthCheck(string filePath)
        {
            _filePath = filePath;
            _HealthCheckData = new Dictionary<string, object>
            {
                { filePath, _filePath }
            };
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var testFile = $"{_filePath}\\test.txt";
                var fileStream = File.Create(testFile);
                fileStream.Close();
                File.Delete(testFile);

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (System.Exception e)
            {

                switch (context.Registration.FailureStatus)
                {
                    case HealthStatus.Degraded:
                        return Task.FromResult(HealthCheckResult.Degraded("Issue writing to file path.",
                            e, _HealthCheckData));
                    case HealthStatus.Healthy:
                        return Task.FromResult(HealthCheckResult.Healthy("Issue writing to file path.",   _HealthCheckData));
                    default:
                        return Task.FromResult(HealthCheckResult.Unhealthy("Issue writing to file path.",
                            e, _HealthCheckData));
                }
            }
        }
    }
}
