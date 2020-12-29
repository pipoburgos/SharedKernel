using HealthChecks.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Reporting;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Domain.Security;
using SharedKernel.Infrastructure.HealthChecks;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Reporting;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            return services
                .AddLogging()
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
                .AddTransient<IBase64, Base64>()
                .AddTransient<IBinarySerializer, BinarySerializer>()
                .AddTransient<ICulture, ThreadUiCulture>()
                .AddTransient<ICustomLogger, DefaultCustomLogger>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IEncryptionHexHelper, EncryptionHexHelper>()
                .AddTransient<IGuid, GuidGenerator>()
                .AddTransient<IIdentityService, HttpContextAccessorIdentityService>()
                .AddTransient<IRandomNumberGenerator, RandomNumberGenerator>()
                .AddTransient<IReportRenderer, ReportRenderer>()
                .AddTransient<ISemaphore, CustomSemaphore>()
                .AddTransient<ISha256, Sha256>()
                .AddTransient<IStreamHelper, StreamHelper>()
                .AddTransient<IStringHelper, StringHelper>()
                .AddTransient<ITripleDes, TripleDes>()
                .AddTransient<IWeb, WebUtils>();
        }

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
                //.AddCheck<CpuHealthCheck>("CPU", tags: new[] { "System", "CPU" })
                .AddCheck("Allocated RAM", new ProcessAllocatedMemoryHealthCheck(50))
                //.AddDiskStorageHealthCheck(opts =>
                //{
                //    opts.AddDrive("C:\\", 100_1000);
                //}, "Disk Storage", HealthStatus.Degraded, new[] { "Disk" })
                .AddPrivateMemoryHealthCheck(600_000_000, "Private Ram Memory", HealthStatus.Degraded, new[] { "RAM" });

            return services;
        }
    }
}