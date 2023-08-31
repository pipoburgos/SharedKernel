using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure.EntityFrameworkCore;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreInMemoryUnitOfWorkAsync<TUnitOfWork, TDbContext>(this IServiceCollection services,
        string connectionString) where TDbContext : DbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWorkAsync
    {
        return services
            .AddEntityFramework()
            .AddDbContext<TDbContext>(a => a
                .UseInMemoryDatabase(connectionString))
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>))
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped)
#endif
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }

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
