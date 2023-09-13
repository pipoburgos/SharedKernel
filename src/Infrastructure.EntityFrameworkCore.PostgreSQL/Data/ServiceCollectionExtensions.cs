using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgreSqlDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext, IDbContextAsync
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention(), serviceLifetime)
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgreSqlUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddEntityFrameworkCorePostgreSqlDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }

    /// <summary> Add service Postgis into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgisDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
                    .UseNetTopologySuite()
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention(), serviceLifetime)
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
                .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service Postgis into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgisUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddEntityFrameworkCorePostgisDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
