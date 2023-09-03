using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;

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
            .AddSharedKernel()
            .AddTransient<IAuditableService, AuditableService>()
            .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>));
    }
}
