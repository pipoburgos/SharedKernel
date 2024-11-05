using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreInMemoryDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext
    {
        return services
            .AddHostedService<DbContextMigrateHostedService<TDbContext>>()
            .AddEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseInMemoryDatabase(connectionString), serviceLifetime)
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreInMemoryUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddEntityFrameworkCoreInMemoryDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
