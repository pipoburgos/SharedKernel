using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.ApacheActiveMq;
using SharedKernel.Infrastructure.Cqrs.Commands.ApacheActiveMq;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Cqrs.Commands.RabbitMq;
using SharedKernel.Infrastructure.Cqrs.Commands.Redis;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.RabbitMq;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Infrastructure.System;
using StackExchange.Redis;
using System;
using System.Reflection;
using RedisCommandsConsumer = SharedKernel.Infrastructure.Cqrs.Commands.Redis.RedisCommandsConsumer;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    /// <summary>  </summary>
    public static class CommandServiceExtensions
    {
        /// <summary>  </summary>
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Type commandHandlerType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddCommandsHandlers(commandHandlerType.Assembly, serviceLifetime);
        }

        /// <summary>  </summary>
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services
                .AddRequests<CommandRequest>(assembly, nameof(CommandRequest.GetUniqueName), false, serviceLifetime)
                .AddFromAssembly(assembly, serviceLifetime, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

        /// <summary>  </summary>
        public static IServiceCollection AddInMemoryCommandBus(this IServiceCollection services)
        {
            return services
                .AddHostedService<QueuedHostedService>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddTransient<ICommandBus, InMemoryCommandBus>();
        }

        /// <summary>  </summary>
        public static IServiceCollection AddApacheActiveMqCommandBusAsync(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApacheActiveMqConfiguration>(configuration.GetSection(nameof(ApacheActiveMqConfiguration)));

            //services
            //    .AddHealthChecks()
            //    .AddApacheMq(brokerUri, "Apache ActiveMq Event Bus", tags: new[] { "Event Bus", "Apache", "ActiveMq" });

            return services
                .AddHostedService<ApacheActiveMqConsumer>()
                .AddTransient<ICommandBusAsync, ApacheActiveMqCommandBusAsync>();
        }

        /// <summary>  </summary>
        public static IServiceCollection AddRedisCommandBusAsync(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddRedis(GetRedisConfiguration(configuration), "Redis Command Bus", tags: new[] { "Command Bus", "Redis" });

            return services
                .AddHostedService<RedisCommandsConsumer>()
                .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddTransient<ICommandBusAsync, RedisCommandBusAsync>()
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddPollyRetry(configuration);
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }

        /// <summary>  </summary>
        public static IServiceCollection AddRabbitMqCommandBusAsync(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

            services
                .AddHealthChecks()
                .AddRabbitMQ(
                    (sp, _) => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                    "RabbitMq Command Bus", tags: new[] { "Command Bus", "RabbitMq" });

            return services
                .AddHostedService<RabbitMqEventBusConfiguration>()
                .AddTransient<ICommandBusAsync, RabbitMqCommandBusAsync>()
                .AddTransient<RabbitMqConnectionFactory>()
                .AddTransient<RabbitMqConsumer>();
        }
    }
}