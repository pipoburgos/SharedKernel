using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Events.Redis;
using SharedKernel.Infrastructure.Validators;
using StackExchange.Redis;
using System;
using System.Reflection;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventBusServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEvents(this IServiceCollection services,
            Type eventType)
        {
            return services.AddDomainEvents(eventType.Assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="domainAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEvents(this IServiceCollection services, Assembly domainAssembly)
        {
            DomainEventsInformation.Register(domainAssembly);
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="delayTimeSpan">Delay TimeSpan to execute domain events queue. Default 50ms</param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, TimeSpan? delayTimeSpan = null)
        {
            return services
                .AddHostedService<InMemoryBackgroundService>()
                .AddSingleton(s => new DomainEventsToExecute(s.GetService<ICustomLogger<DomainEventsToExecute>>(), delayTimeSpan ?? TimeSpan.FromMilliseconds(50)))
                .AddEventBus()
                .AddScoped<InMemoryDomainEventsConsumer>()
                .AddScoped<IEventBus, InMemoryEventBus>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
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
        public static IServiceCollection AddRedisEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddRedis(GetRedisConfiguration(configuration), "Redis Event Bus", tags: new[] { "Event Bus", "Redis" });

            return services
                .AddHostedService<RedisDomainEventsConsumer>()
                .AddEventBus()
                .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddScoped<IEventBus, RedisEventBus>();
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddScoped<ExecuteMiddlewaresService>()
                .AddScoped<DomainEventMediator>()
                .AddScoped<DomainEventJsonSerializer>()
                .AddScoped<DomainEventJsonDeserializer>();
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }
    }
}
