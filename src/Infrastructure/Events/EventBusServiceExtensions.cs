using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Reflection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Security;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Events.Redis;
using SharedKernel.Infrastructure.Events.Shared;
using SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents;
using SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.Validators;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventBusServiceExtensions
    {
        #region Domain Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="eventType"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEvents(this IServiceCollection services,
            Type eventType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddDomainEvents(eventType.Assembly, serviceLifetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="domainAssembly"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEvents(this IServiceCollection services, Assembly domainAssembly,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var domainTypes = GetDomainTypes(domainAssembly);
            foreach (var eventType in domainTypes)
            {
                var eventName = GetEventName(eventType);

                services.Add(new ServiceDescriptor(typeof(IDomainEventType),
                    _ => new DomainEventType(eventName, eventType), serviceLifetime));
            }

            return services;
        }

        private static string GetEventName(Type eventType)
        {
            var instance = ReflectionHelper.CreateInstance<DomainEvent>(eventType);
            return eventType.GetMethod(nameof(DomainEvent.GetEventName))?.Invoke(instance, null)?.ToString();
        }

        private static IEnumerable<Type> GetDomainTypes(Assembly domainAssembly)
        {
            return domainAssembly
                .GetTypes()
                .Where(p => typeof(DomainEvent).IsAssignableFrom(p) && !p.IsAbstract);
        }

        #endregion

        #region Domain Events Subscribers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services, Type type,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddDomainEventsSubscribers(type.Assembly, serviceLifetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
            Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var classTypes = assembly.GetTypes().Select(t => t.GetTypeInfo()).Where(t => t.IsClass);

            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces
                    .Select(i => i.GetTypeInfo())
                    .Where(i => i.IsAssignableFrom(typeof(DomainEventSubscriber<>)))
                    .ToList();

                foreach (var handlerInterfaceType in interfaces)
                {
                    var @interface = handlerInterfaceType.AsType();
                    var @class = type.AsType();
                    services.Add(new ServiceDescriptor(@interface, @class, serviceLifetime));
                    services.Add(new ServiceDescriptor(@class, @class, serviceLifetime));
                }
            }

            return services;
        }


        /// <summary> Call just before compiling service collections </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventSubscribers(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var subscribers = services.Where(s => s.ServiceType.IsAssignableFrom(typeof(DomainEventSubscriber<>))).ToList();

            foreach (var subscriber in subscribers)
            {
                var subscriberClass = subscriber.ImplementationType;

                if (subscriberClass is null)
                    continue;

                var eventType = subscriberClass.BaseType?.GenericTypeArguments.Single();

                services.Add(new ServiceDescriptor(typeof(IDomainEventSubscriberType),
                    _ => new DomainEventSubscriberType(subscriberClass, eventType), serviceLifetime));
            }

            return services;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
        {
            return services
                .AddHostedService<InMemoryBackgroundService>()
                .AddSingleton<IInMemoryDomainEventsConsumer, InMemoryDomainEventsConsumer>()
                .AddEventBus()
                .AddScoped<IEventBus, InMemoryEventBus>();
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
                    sp => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                    "RabbitMq Event Bus", tags: new[] { "Event Bus", "RabbitMq" });

            return services
                .AddHostedService<RabbitMqEventBusConfiguration>()
                .AddEventBus()
                //.AddTransient<MsSqlEventBus, MsSqlEventBus>() // Failover
                .AddScoped<IEventBus, RabbitMqEventBus>()
                .AddScoped<RabbitMqConnectionFactory>()
                .AddScoped<RabbitMqDomainEventsConsumer>();
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
                .AddHostedService<RedisDomainEventsConsumer>()
                .AddEventBus()
                .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddScoped<IEventBus, RedisEventBus>()
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddPollyRetry(configuration);
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient<IIdentityService, HttpContextAccessorIdentityService>()
                .AddTransient<IExecuteMiddlewaresService, ExecuteMiddlewaresService>()
                .AddTransient<IDomainEventMediator, DomainEventMediator>()
                .AddTransient<IDomainEventJsonSerializer, DomainEventJsonSerializer>()
                .AddTransient<IDomainEventJsonDeserializer, DomainEventJsonDeserializer>()
                .AddSingleton<IDomainEventProviderFactory, DomainEventProviderFactory>()
                .AddSingleton<IDomainEventSubscriberProviderFactory, DomainEventSubscriberProviderFactory>();
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }
    }
}
