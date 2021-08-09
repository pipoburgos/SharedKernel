using HealthChecks.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SharedKernel.Infrastructure.HealthChecks
{
    /// <summary>
    /// 
    /// </summary>
    public static class AddSharedKernelHealthChecksExtensions
    {
        /// <summary>
        /// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.System/DependencyInjection/SystemHealthCheckBuilderExtensions.cs
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernelHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("google.com", new PingHealthCheck("google.com", 200),
                    tags: new[] { "System", "Ping", "google.com" })
                .AddCheck("bing.com", new PingHealthCheck("bing.com", 200), tags: new[] { "System", "Ping", "bing.com" })
                .AddCheck<RamHealthCheck>("RAM", tags: new[] { "System", "RAM" })
                .AddCheck("Allocated RAM", new ProcessAllocatedMemoryHealthCheck(50))
                .AddPrivateMemoryHealthCheck(600_000_000, "Private Ram Memory", HealthStatus.Degraded, new[] { "RAM" });

            return services;
        }
    }
}