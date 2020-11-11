using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    public static class CommandServiceExtensions
    {
        public static IServiceCollection AddCommandsHandlers(this IServiceCollection services,
            Assembly assembly)
        {
            return services.AddFromAssembly(assembly, typeof(ICommandRequestHandler<>), typeof(ICommandRequestHandler<,>));
        }
    }
}