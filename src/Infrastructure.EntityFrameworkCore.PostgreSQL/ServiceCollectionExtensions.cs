using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;
#if !NET6_0 && !NET7_0 && !NET8_0
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
#else
using System;
#endif

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service PostgreSQL into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCorePostgreSql<TContext>(this IServiceCollection services,
        string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
    {
        services.AddPostgreSqlHealthChecks(connectionString, "Postgre EFCore");

        services.AddCommonDataServices();

#if !NET6_0 && !NET7_0 && !NET8_0

        services.AddDbContext<TContext>(p => p.UseNpgsql(connectionString), serviceLifetime);

        services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
        services.AddDbContext<TContext>(p => p.UseNpgsql(connectionString, e => e
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), default)), serviceLifetime);

        services.AddDbContextFactory<TContext>(lifetime: serviceLifetime);
#endif

        return services;
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
