using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.MassTransit.Events;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMassTransitEventBus(this IServiceCollection services)
    {
        services.AddOptions<MassTransitHostOptions>();

        return services
            .AddTransient<IEventBus, MassTransitEventBus>()
            .AddTransient<MassTransitConsumer>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitConsumer>();
                x.UsingInMemory();
            });
    }
}