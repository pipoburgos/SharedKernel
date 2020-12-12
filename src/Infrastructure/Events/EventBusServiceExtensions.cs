using System;
using System.Linq;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Events;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.MassTransit;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Events.Redis;
using SharedKernel.Infrastructure.Validators;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Events
{
    public static class EventBusServiceExtensions
    {
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
                .AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddTransient<IEventBus, RedisEventBus>();
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient<DomainEventMediator>()
                .AddTransient<DomainEventsInformation>()
                .AddTransient<DomainEventSubscribersInformation>()
                .AddTransient<DomainEventJsonSerializer>()
                .AddTransient<DomainEventJsonDeserializer>();
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }

        public static IServiceCollection AddMassTransitEventBus(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            var config = new RabbitMqConfigParams();
            configuration.GetSection("RabbitMq").Bind(config);

            services
                .AddHealthChecks()
                .AddRabbitMQ(
                    sp => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                    "MassTransit RabbitMq Event Bus", tags: new[] { "Event Bus", "RabbitMq" });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = check => check.Tags.Contains("ready");
            });

            return services
                .AddMassTransitHostedService()
                .AddTransient<IEventBus, MassTransitEventBus>()
                .AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(new Uri($"rabbitmq://{config.HostName}:{config.Port}"), h =>
                        {
                            h.Username(config.Username);
                            h.Password(config.Password);
                        });

                        cfg.MessageTopology.SetEntityNameFormatter(new EnvironmentNameFormatter(cfg.MessageTopology.EntityNameFormatter));
                    });

                    var timeout = TimeSpan.FromSeconds(10);
                    var serviceAddress = new Uri($"rabbitmq://{config.HostName}:{config.Port}/{config.ExchangeName}");

                    x.AddConsumers(assemblies);

                    foreach (var assembly in assemblies)
                    {
                        foreach (var subscriber in assembly.GetTypes().Where(s => s.IsAssignableFrom(typeof(DomainEventSubscriber<>))))
                        {
                            x.AddRequestClient(subscriber, serviceAddress, timeout);
                            //x.AddRequestClient<Usuario>(subscriber, serviceAddress, timeout);
                        }
                    }
                });
        }
    }
}
