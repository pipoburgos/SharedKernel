using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.ApacheActiveMq;
using SharedKernel.Infrastructure.Events.ApacheActiveMq;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Events.Redis;
using SharedKernel.Infrastructure.Events.Synchronous;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.RabbitMq;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Infrastructure.System;
using StackExchange.Redis;
using System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventBusServiceExtensions
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSynchronousEventBus(this IServiceCollection services)
        {
            return services
                .AddTransient<IEventBus, SynchronousEventBus>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddHostedService<InMemoryBackgroundService>()
                .AddSingleton<EventQueue>()
                .AddTransient<IInMemoryDomainEventsConsumer, InMemoryDomainEventsConsumer>()
                .AddTransient<IEventBus, InMemoryEventBus>()
                .AddPollyRetry(configuration);
        }

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
                    (sp, _) => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                    "RabbitMq Event Bus", tags: new[] { "Event Bus", "RabbitMq" });

            return services
                .AddHostedService<RabbitMqEventBusConfiguration>()
                //.AddTransient<MsSqlEventBus, MsSqlEventBus>() // Failover
                .AddTransient<IEventBus, RabbitMqEventBus>()
                .AddTransient<RabbitMqConnectionFactory>()
                .AddTransient<RabbitMqConsumer>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisEventBus(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddRedis(GetRedisConfiguration(configuration), "Redis Event Bus", tags: new[] { "Event Bus", "Redis" });

            return services
                .AddHostedService<RedisCommandsConsumer>()
                .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddTransient<IEventBus, RedisEventBus>()
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddPollyRetry(configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApacheActiveMqEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApacheActiveMqConfiguration>(configuration.GetSection(nameof(ApacheActiveMqConfiguration)));

            //services
            //    .AddHealthChecks()
            //    .AddApacheMq(brokerUri, "Apache ActiveMq Event Bus", tags: new[] { "Event Bus", "Apache", "ActiveMq" });

            return services
                .AddHostedService<ApacheActiveMqConsumer>()
                .AddTransient<IEventBus, ApacheActiveMqEventBus>()
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>));
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }
    }
}
