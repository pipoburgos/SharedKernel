using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Application.UnitOfWorks;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public static class CommandsSaveAndPublish
{
    public static List<Type> CommandHandlersShouldInjectEventBusAndUnitOfWork(
        this Assembly assembly)
    {
        var handlers = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommandRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandRequestHandler<,>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var handler in handlers)
        {
            var constructor = handler.GetConstructors()
                .FirstOrDefault();

            if (constructor is null)
            {
                failingTypes.Add(handler);
                continue;
            }

            var parameters = constructor.GetParameters();

            var hasEventBus = parameters.Any(p =>
                typeof(IEventBus).IsAssignableFrom(p.ParameterType));

            var hasUnitOfWork = parameters.Any(p =>
                typeof(IUnitOfWork).IsAssignableFrom(p.ParameterType));

            if (!hasEventBus || !hasUnitOfWork)
                failingTypes.Add(handler);
        }

        return failingTypes;
    }

}
