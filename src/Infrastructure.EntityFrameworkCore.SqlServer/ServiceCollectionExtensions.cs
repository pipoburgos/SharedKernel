using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.UnitOfWorks;
#if !NET6_0 && !NET7_0 && !NET8_0
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
#endif

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreSqlServerUnitOfWorkAsync<TUnitOfWork, TDbContext>(this IServiceCollection services,
        string connectionString) where TDbContext : DbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWorkAsync
    {
        return services
            .AddSqlServerHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework()
            .AddDbContext<TDbContext>(a => a
                .UseSqlServer(connectionString, b => b
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!)))
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>))
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped)
#endif
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
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
