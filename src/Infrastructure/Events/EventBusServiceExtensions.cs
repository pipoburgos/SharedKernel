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

namespace SharedKernel.Infrastructure.Events
{
    public static class EventBusServiceExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services,
            Type eventType)
        {
            return services.AddDomainEvents(eventType.Assembly);
        }

        public static IServiceCollection AddDomainEvents(this IServiceCollection services, Assembly domainAssembly)
        {
            DomainEventsInformation.Register(domainAssembly);
            return services;
        }

        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
        {
            return services
                .AddEventBus()
                .AddTransient<IEventBus, InMemoryEventBus>();
        }

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
                .AddTransient<IEventBus, RabbitMqEventBus>()
                .AddTransient<RabbitMqPublisher>()
                .AddTransient<RabbitMqConnectionFactory>()
                .AddTransient<RabbitMqDomainEventsConsumer>();
        }

        public static IServiceCollection AddRedisEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddRedis(GetRedisConfiguration(configuration), "Redis Event Bus", tags: new[] { "Event Bus", "Redis" });

            return services
                .AddHostedService<RedisConsumer>()
                .AddEventBus()
                .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddTransient<IEventBus, RedisEventBus>();
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient<DomainEventMediator>()
                .AddTransient<DomainEventSubscribersInformation>()
                .AddTransient<DomainEventJsonSerializer>()
                .AddTransient<DomainEventJsonDeserializer>();
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }
    }
}
