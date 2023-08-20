using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure.EntityFrameworkCore;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register common Ef Core data services. </summary>
    public static IServiceCollection AddEntityFramework(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
            .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>))
            .AddTransient<IIdentityService, DefaultIdentityService>()
            .AddTransient<IDateTime, MachineDateTime>()
            .AddTransient<IGuid, GuidGenerator>()
            .AddTransient<IAuditableService, AuditableService>()
            .AddTransient<IValidatableObjectService, ValidatableObjectService>();
    }
}
