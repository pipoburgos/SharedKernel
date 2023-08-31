using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.UnitOfWorks;
#if !NET6_0 && !NET7_0 && !NET8_0
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
#endif

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgreSqlUnitOfWorkAsync<TUnitOfWork, TDbContext>(this IServiceCollection services,
        string connectionString) where TDbContext : DbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWorkAsync
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework()
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention())
#if !NET6_0 && !NET7_0 && !NET8_0
                .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>))
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped)
#endif
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }


    /// <summary> Add service Postgis into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgisUnitOfWorkAsync<TUnitOfWork, TDbContext>(this IServiceCollection services,
            string connectionString) where TDbContext : DbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWorkAsync
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework()
            .AddDbContext<TDbContext>(a => a
                .UseNpgsql(connectionString, b => b
                    .UseNetTopologySuite()
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!))
                .UseSnakeCaseNamingConvention())
#if !NET6_0 && !NET7_0 && !NET8_0
                .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>))
#else
                .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped)
#endif
                .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }

    /// <summary>  </summary>
    internal static IServiceCollection AddPostgreSqlHealthChecks(this IServiceCollection services,
        string connectionString, string name, params string[] tags)
    {
        var tagsList = tags.ToList();
        tagsList.Add("Postgre");
        tagsList.Add("DB");
        tagsList.Add("Sql");

        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", _ => { }, name, HealthStatus.Unhealthy, tagsList);

        return services;
    }
}
