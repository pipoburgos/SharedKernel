using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;

namespace SharedKernel.Infrastructure.EntityFrameworkCore;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register common Ef Core data services. </summary>
    public static IServiceCollection AddEntityFramework(this IServiceCollection services)
    {
        return services
            .AddSharedKernel()
            .AddTransient<IAuditableService, AuditableService>()
            .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>));
    }
}
