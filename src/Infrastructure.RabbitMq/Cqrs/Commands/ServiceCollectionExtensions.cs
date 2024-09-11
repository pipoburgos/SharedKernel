using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Infrastructure.RabbitMq.Cqrs.Commands;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddRabbitMqCommandBusAsync(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

        services
            .AddHealthChecks()
            .AddRabbitMQ(
                (sp, _) => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                "RabbitMq Command Bus", tags: ["Command Bus", "RabbitMq"]);

        return services
            .AddHostedService<RabbitMqEventBusConfiguration>()
            .AddTransient<ICommandBusAsync, RabbitMqCommandBusAsync>()
            .AddTransient<RabbitMqConnectionFactory>()
            .AddTransient<RabbitMqConsumer>();
    }
}