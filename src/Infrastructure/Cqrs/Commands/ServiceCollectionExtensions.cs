using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Commands;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelCommandsHandlers(this IServiceCollection services,
        Type commandHandlerType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        return services.AddSharedKernelCommandsHandlers(commandHandlerType.Assembly, serviceLifetime);
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelCommandsHandlers(this IServiceCollection services,
        Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        return services
            .AddSharedKernelRequests<CommandRequest>(assembly, nameof(CommandRequest.GetUniqueName), false, serviceLifetime)
            .AddSharedKernelFromAssembly(assembly, serviceLifetime, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelInMemoryCommandBus(this IServiceCollection services)
    {
        return services
            .AddHostedService<QueuedHostedService>()
            .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
            .AddTransient<ICommandBus, InMemoryCommandBus>();
    }
}
