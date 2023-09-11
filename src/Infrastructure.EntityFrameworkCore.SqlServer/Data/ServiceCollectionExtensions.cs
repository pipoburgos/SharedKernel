using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreSqlServerDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext
    {
        return services
            .AddSqlServerHealthChecks(connectionString, $"Postgis EFCore {typeof(TDbContext)}")
            .AddEntityFramework()
            .AddDbContext<TDbContext>(a => a
                .UseSqlServer(connectionString, b => b
#if NET6_0 || NET7_0 || NET8_0
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!)), serviceLifetime)
#if !NET6_0 && !NET7_0 && !NET8_0
            .AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            .AddDbContextFactory<TDbContext>(lifetime: ServiceLifetime.Scoped);
#endif
    }

    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreSqlServerUnitOfWork<TUnitOfWork, TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : EntityFrameworkDbContext, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddEntityFrameworkCoreSqlServerDbContext<TDbContext>(connectionString, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
