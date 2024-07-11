using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddMassTransitCommandBusAsync(this IServiceCollection services)
    {
        services.AddOptions<MassTransitHostOptions>();

        return services
            .AddTransient<ICommandBusAsync, MassTransitCommandBusAsync>()
            .AddTransient<MassTransitConsumer>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitConsumer>();
                x.UsingInMemory();
            });
    }
}