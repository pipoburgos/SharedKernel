using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.Synchronous;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Events;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    #region Domain Events Subscribers

    /// <summary>  </summary>
    public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
        Type subscribersType, Type domainType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        return services.AddDomainEventsSubscribers(subscribersType.Assembly, domainType.Assembly, serviceLifetime);
    }

    /// <summary>  </summary>
    public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
        Assembly subscribersAssembly, Assembly domainAssembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        return services
            .AddRequests<DomainEvent>(domainAssembly, nameof(DomainEvent.GetUniqueName), true, serviceLifetime)
            .AddFromAssembly(subscribersAssembly, serviceLifetime, typeof(IDomainEventSubscriber<>));
    }

    #endregion

    /// <summary>  </summary>
    public static IServiceCollection AddSynchronousEventBus(this IServiceCollection services)
    {
        return services
            .AddTransient<IEventBus, SynchronousEventBus>();
    }

    /// <summary>  </summary>
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddHostedService<InMemoryBackgroundService>()
            .AddSingleton<EventQueue>()
            .AddTransient<IInMemoryDomainEventsConsumer, InMemoryDomainEventsConsumer>()
            .AddTransient<IEventBus, InMemoryEventBus>();
    }
}
