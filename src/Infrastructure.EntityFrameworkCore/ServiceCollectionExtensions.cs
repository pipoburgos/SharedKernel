using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validator;

namespace SharedKernel.Infrastructure.EntityFrameworkCore;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register common Ef Core data services. </summary>
    public static IServiceCollection AddEntityFramework<TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime) where TDbContext : EntityFrameworkDbContext
    {
        services.Add(new ServiceDescriptor(typeof(IDbContextAsync), sp => sp.GetRequiredService<TDbContext>(),
            serviceLifetime));

        return services
            .AddTransient<IAuditableService, AuditableService>()
            .AddTransient<IClassValidatorService, ClassValidatorService>()
            .AddTransient<IDateTime, MachineDateTime>()
            .AddTransient<IEntityAuditableService, EntityAuditableService>()
            .AddTransient<IGuid, GuidGenerator>()
            .AddTransient<IIdentityService, DefaultIdentityService>()
            .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
            .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>));
    }
}
