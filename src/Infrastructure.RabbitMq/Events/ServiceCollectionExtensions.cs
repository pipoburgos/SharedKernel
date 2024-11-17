using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.RabbitMq.Events;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

        services
            .AddHealthChecks()
            .AddRabbitMQ(
                (sp, _) => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>()
                    .CreateConnectionAsync().GetAwaiter().GetResult(), "RabbitMq Event Bus",
                tags: ["Event Bus", "RabbitMq"]);

        return services
            .AddHostedService<RabbitMqBackground>()
            //.AddTransient<MsSqlEventBus, MsSqlEventBus>() // Failover
            .AddTransient<IEventBus, RabbitMqEventBus>()
            .AddTransient<RabbitMqConnectionFactory>()
            .AddTransient<RabbitMqConsumer>();
    }
}