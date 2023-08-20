using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;
#if !NET6_0 && !NET7_0 && !NET8_0
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
#else
using System;
#endif

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddEntityFrameworkCoreSqlServer<TContext>(this IServiceCollection services,
        string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, "Sql Server EFCore",
                HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

        services.AddCommonDataServices();

#if !NET6_0 && !NET7_0 && !NET8_0

        services.AddDbContext<TContext>(s => s.UseSqlServer(connectionString), serviceLifetime);

        services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
        services.AddDbContext<TContext>(s => s.UseSqlServer(connectionString, e => e
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), default)), serviceLifetime);

        services.AddDbContextFactory<TContext>(lifetime: serviceLifetime);
#endif

        return services;
    }

    /// <summary>  </summary>
    internal static IServiceCollection AddSqlServerHealthChecks(this IServiceCollection services,
        string connectionString, string name, params string[] tags)
    {
        var tagsList = tags.ToList();
        tagsList.Add("SqlServer");
        tagsList.Add("DB");
        tagsList.Add("Sql");

        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, name, HealthStatus.Unhealthy, tagsList);

        return services;
    }
}
