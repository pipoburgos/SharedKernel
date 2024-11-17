using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddSharedKernelMassTransitCommandBusAsync(this IServiceCollection services,
        Action<IBusRegistrationConfigurator> configure)
    {
        services.AddOptions<MassTransitHostOptions>();

        return services
            .AddTransient<ICommandBusAsync, MassTransitCommandBusAsync>()
            .AddTransient<MassTransitCommandConsumer>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitCommandConsumer>();
                configure(x);
            });
    }
}