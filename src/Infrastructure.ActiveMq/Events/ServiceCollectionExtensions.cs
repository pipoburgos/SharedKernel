using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Logging;

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
            .AddHostedService<ActiveMqConsumer>()
            .AddTransient<IEventBus, ActiveMqEventBus>()
            .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>));
    }
}