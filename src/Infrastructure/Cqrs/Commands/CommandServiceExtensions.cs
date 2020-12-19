using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    public static class CommandServiceExtensions
    {
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Assembly applicationAssembly)
        {
            return services.AddFromAssembly(applicationAssembly, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Type commandHandlerType)
        {
            return services.AddFromAssembly(commandHandlerType.Assembly, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }

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
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
        }
    }
}