using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.HealthChecks
{
    /// <summary>
    /// 
    /// </summary>
    public class RamHealthCheck : IHealthCheck
    {
        private readonly ICustomLogger<RamHealthCheck> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public RamHealthCheck(ICustomLogger<RamHealthCheck> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
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

            var result = new HealthCheckResult(status, $"RAM {megabytes} megabytes");

            return Task.FromResult(result);
        }
    }
}
