using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;
using System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommandServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="applicationAssembly"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Assembly applicationAssembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddFromAssembly(applicationAssembly, serviceLifetime,
                typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="commandHandlerType"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Type commandHandlerType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddFromAssembly(commandHandlerType.Assembly, serviceLifetime,
                typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryCommandBus(this IServiceCollection services)
        {
            return services
                .AddCommandBus()
                .AddTransient<ICommandBus, InMemoryCommandBus>();
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