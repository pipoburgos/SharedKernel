using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Domain.Security;
using SharedKernel.Infrastructure.HealthChecks;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            services.AddLogging();

            services.AddHealthChecks()
                .AddCheck("google.com", new PingHealthCheck("google.com", 200), tags: new[] { "system", "ping", "google.com" })
                .AddCheck("bing.com", new PingHealthCheck("bing.com", 200), tags: new[] { "system", "ping", "bing.com" })
                .AddCheck<RamHealthcheck>("RAM", tags: new[] { "system", "RAM" })
                .AddCheck<CpuHealthCheck>("CPU", tags: new[] { "system", "CPU" });

            return services
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
                .AddTransient<ISemaphore, CustomSemaphore>()
                .AddTransient<ISha256, Sha256>()
                .AddTransient<IStreamHelper, StreamHelper>()
                .AddTransient<IStringHelper, StringHelper>()
                .AddTransient<ITripleDes, TripleDes>()
                .AddTransient<IWeb, WebUtils>();
        }
    }
}