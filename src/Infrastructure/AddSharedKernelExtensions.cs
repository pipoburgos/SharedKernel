using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.System;
using SharedKernel.Application.System.Threading;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure;

/// <summary> . </summary>
public static class AddSharedKernelExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        return services
            .AddSharedKernelLogging()
            .AddSharedKernelOptions()
            .AddTransient<IBase64, Base64>()
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<ICulture, ThreadUiCulture>()
            .AddTransient<IDateTime, MachineDateTime>()
            .AddTransient<IMd5Encryptor, Md5Encryptor>()
            .AddTransient<IGuid, GuidGenerator>()
            .AddTransient<IIdentityService, DefaultIdentityService>()
            .AddTransient<IParallel, System.Threading.Parallel>()
            .AddTransient<IRandomNumberGenerator, RandomNumberGenerator>()
            .AddTransient<ISha256, Sha256>()
            .AddTransient<IStreamHelper, StreamHelper>()
            .AddTransient<IStringHelper, StringHelper>()
            .AddTransient<ITripleDes, TripleDes>()
            .AddTransient<IWeb, WebUtils>();
    }
}
