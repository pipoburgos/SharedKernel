using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.MassTransit.Events;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddSharedKernelMassTransitEventBus(this IServiceCollection services,
        Action<IBusRegistrationConfigurator> configure)
    {
        services.AddOptions<MassTransitHostOptions>();

        return services
            .AddTransient<IEventBus, MassTransitEventBus>()
            .AddTransient<MassTransitEventConsumer>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitEventConsumer>();
                configure(x);
            });
    }
}