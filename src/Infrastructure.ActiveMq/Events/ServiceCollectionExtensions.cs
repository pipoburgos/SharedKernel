using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.ActiveMq.Events;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddActiveMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddActiveMqHealthChecks(configuration, "ActiveMq Event Bus", "ActiveMq", "EventBus")
            .AddHostedService<ActiveMqTopicsConsumer>()
            .AddTransient<IEventBus, ActiveMqEventBus>();
    }
}