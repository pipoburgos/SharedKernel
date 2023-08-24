using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> IMPORTANT!!! Add modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration()) </summary>
    public static IServiceCollection AddEntityFrameworkFailoverMiddleware<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        return services
            .AddTransient<IRequestFailoverRepository, EntityFrameworkCoreRequestFailoverRepository<TContext>>()
            .AddTransient<FailoverCommonLogic>()
            .AddTransient(typeof(IMiddleware<>), typeof(FailoverMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(FailoverMiddleware<,>));
    }
}
