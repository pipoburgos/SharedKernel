using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddSharedKernelEntityFrameworkCorePostgreSqlDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext, IDbContextAsync
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"PostgreSql EFCore {typeof(TDbContext)}")
            .AddSharedKernelEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention(), serviceLifetime)
#if !NET6_0_OR_GREATER
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddSharedKernelEntityFrameworkCorePostgreSqlUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelEntityFrameworkCorePostgreSqlDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }

    /// <summary> Add service Postgis into IServiceCollection. </summary>
    public static IServiceCollection AddSharedKernelEntityFrameworkCorePostgisDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddSharedKernelEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
                    .UseNetTopologySuite()
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention(), serviceLifetime)
#if !NET6_0_OR_GREATER
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
                .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service Postgis into IServiceCollection. </summary>
    public static IServiceCollection AddSharedKernelEntityFrameworkCorePostgisUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelEntityFrameworkCorePostgisDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
