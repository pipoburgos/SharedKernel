using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Application.System.Threading;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.UnitOfWorks;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validator;

namespace SharedKernel.Infrastructure;

/// <summary>  </summary>
public static class AddSharedKernelExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
            .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
            .AddTransient<IEntityAuditableService, EntityAuditableService>()
            .AddTransient<IBase64, Base64>()
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<IClassValidatorService, ClassValidatorService>()
            .AddTransient<ICulture, ThreadUiCulture>()
            .AddTransient<ICustomLogger, DefaultCustomLogger>()
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
