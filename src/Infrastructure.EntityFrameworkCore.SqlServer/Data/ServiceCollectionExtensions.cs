using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add service Sql Server into IServiceCollection. </summary>
    public static IServiceCollection AddEntityFrameworkCoreSqlServerDbContext<TDbContext>(
        this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : EntityFrameworkDbContext
    {
        return services
            .AddSqlServerHealthChecks(connectionString, $"SqlServer EFCore {typeof(TDbContext)}")
            .AddEntityFramework<TDbContext>(serviceLifetime)
            .AddDbContext<TDbContext>(a => a
                .UseSqlServer(connectionString, b => b
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1.0), default!)), serviceLifetime);
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
