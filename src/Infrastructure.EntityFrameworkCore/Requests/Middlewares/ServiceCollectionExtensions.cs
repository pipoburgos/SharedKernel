using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> IMPORTANT!!! Add modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration()) </summary>
    public static IServiceCollection AddSharedKernelEntityFrameworkCoreFailoverRepository<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        return services
            .AddTransient<IRequestFailoverRepository, EntityFrameworkCoreRequestFailoverRepository<TContext>>();
    }
}
