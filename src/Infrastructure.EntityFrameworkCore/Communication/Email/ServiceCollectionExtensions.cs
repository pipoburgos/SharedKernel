using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Communication.Email;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> IMPORTANT!!! Add modelBuilder.ApplyConfiguration(new OutboxMailConfiguration()) </summary>
    public static IServiceCollection AddEntityFrameworkCoreOutboxMailRepository<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        return services
            .AddTransient<IOutboxMailRepository, EntityFrameworkCoreOutboxMailRepository<TContext>>();
    }
}
