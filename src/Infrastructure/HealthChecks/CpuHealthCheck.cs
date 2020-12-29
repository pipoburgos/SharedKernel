using System;
using System.Collections.Generic;
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
    public class CpuHealthCheck : IHealthCheck
    {
        private readonly ICustomLogger<CpuHealthCheck> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CpuHealthCheck(ICustomLogger<CpuHealthCheck> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var time = Process.GetCurrentProcess().TotalProcessorTime;

            _logger.Info($"CPU {time}");

            var status = HealthStatus.Healthy;
            if (time > TimeSpan.FromSeconds(10))
            {
                status = HealthStatus.Degraded;
            }

            if (time > TimeSpan.FromSeconds(40))
            {
                status = HealthStatus.Unhealthy;
            }

            var data = new Dictionary<string, object>
            {
                {"Cpu usage", time}
            };

            var result = new HealthCheckResult(status, null, null, data);

            return await Task.FromResult(result);
        }
    }
}
