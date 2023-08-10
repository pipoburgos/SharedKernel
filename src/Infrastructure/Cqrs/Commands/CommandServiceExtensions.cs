using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Commands.ApacheActiveMq;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.ApacheActiveMq;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;
using System;
using System.Reflection;

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
                .AddRequests<CommandRequest>(assembly, nameof(CommandRequest.GetUniqueName), serviceLifetime)
                .AddFromAssembly(assembly, serviceLifetime, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

        /// <summary>  </summary>
        public static IServiceCollection AddInMemoryCommandBus(this IServiceCollection services)
        {
            return services
                .AddCommandBus()
                .AddTransient<ICommandBus, InMemoryCommandBus>();
        }

        /// <summary>  </summary>
        public static IServiceCollection AddApacheActiveMqCommandBusAsync(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApacheActiveMqConfiguration>(configuration.GetSection(nameof(ApacheActiveMqConfiguration)));

            return services
                .AddCommandBus()
                .AddHostedService<ApacheActiveMqCommandsConsumer>()
                .AddTransient<ICommandBusAsync, ApacheActiveMqCommandBusAsync>()
                .AddTransient<IRequestSerializer, RequestSerializer>()
                .AddTransient<IRequestDeserializer, RequestDeserializer>()
                .AddTransient<IRequestMediator, RequestMediator>();
        }

        private static IServiceCollection AddCommandBus(this IServiceCollection services)
        {
            return services
                .AddHostedService<QueuedHostedService>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddTransient<IExecuteMiddlewaresService, ExecuteMiddlewaresService>()
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
        }
    }
}