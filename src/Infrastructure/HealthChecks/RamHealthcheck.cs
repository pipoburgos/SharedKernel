using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.HealthChecks
{
    public class RamHealthcheck : IHealthCheck
    {
        private readonly ICustomLogger<RamHealthcheck> _logger;

        public RamHealthcheck(ICustomLogger<RamHealthcheck> logger)
        {
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var process = Process.GetCurrentProcess();

            var bytes = process.PeakWorkingSet64;
            var kilobytes = bytes / 1024;
            var megabytes = kilobytes / 1024;

            _logger.Info($"RAM {megabytes}");

            var status = HealthStatus.Healthy;
            if (megabytes > 400)
            {
                status = HealthStatus.Degraded;
            }
            if (megabytes > 1_000)
            {
                status = HealthStatus.Unhealthy;
            }

            var result = new HealthCheckResult(status);

            return await Task.FromResult(result);
        }
    }
}
