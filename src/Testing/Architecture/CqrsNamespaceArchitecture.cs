using FluentValidation;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Requests;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public static class CqrsNamespaceArchitecture
{
    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types
                .Where(t => t != null)
                .Cast<Type>();
        }
    }

    private static bool IsUseCase(Type type)
    {
        return typeof(IRequest).IsAssignableFrom(type) &&
               !typeof(DomainEvent).IsAssignableFrom(type);
    }

    private static bool IsCommand(Type type)
    {
        return type.GetInterfaces().Any(x =>
            x == typeof(ICommandRequest) ||
            (x.IsGenericType &&
             x.GetGenericTypeDefinition() == typeof(ICommandRequest<>)));
    }

    private static bool IsQuery(Type type)
    {
        return type.GetInterfaces().Any(x =>
            x.IsGenericType &&
            x.GetGenericTypeDefinition() == typeof(IQueryRequest<>));
    }

    private static void Check(
        List<string> errors,
        Type type,
        params string[] namespaces)
    {
        if (!namespaces.Any(n => type.Namespace == n))
        {
            errors.Add(
                $"{type.FullName}: {type.Namespace} expected namespace '{string.Join("', '", namespaces)}'");
        }
    }

    private static Type? GetHandler(Type useCase, IEnumerable<Type> types)
    {
        return types.SingleOrDefault(type =>
            type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType &&
                (
                    @interface.GetGenericTypeDefinition() == typeof(ICommandRequestHandler<>) ||
                    @interface.GetGenericTypeDefinition() == typeof(ICommandRequestHandler<,>) ||
                    @interface.GetGenericTypeDefinition() == typeof(IQueryRequestHandler<,>)
                ) &&
                @interface.GetGenericArguments()[0] == useCase));
    }

    private static Type? GetValidator(Type useCase, IEnumerable<Type> types)
    {
        return types.SingleOrDefault(type =>
            type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType &&
                @interface.GetGenericTypeDefinition() == typeof(IValidator<>) &&
                @interface.GetGenericArguments()[0] == useCase));
    }

    private static Type? GetEndpoint(Type useCase, IEnumerable<Type> types)
    {
        return types.SingleOrDefault(type =>
            type.Name == $"{useCase.Name}Endpoint" &&
            type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType &&
                @interface.GetGenericTypeDefinition() == typeof(IEndpoint<>)));
    }

    public static List<string> TestCqrsTypesShouldBeInSameNamespace(
        this IEnumerable<Assembly> assemblies)
    {
        var types = assemblies
            .SelectMany(GetLoadableTypes)
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        var useCases = types
            .Where(IsUseCase)
            .OrderBy(t => t.FullName)
            .ToList();

        var errors = new List<string>();

        if (useCases.Count == 0)
            return errors;

        var projectName = useCases.First().Namespace!.Split('.').First();

        foreach (var useCase in useCases)
        {
            var isCommand = IsCommand(useCase);
            var isQuery = IsQuery(useCase);

            if (!isCommand && !isQuery)
            {
                errors.Add($"{useCase.FullName}: use case must be a command or a query");

                continue;
            }

            var path = isCommand ? "Commands" : "Queries";
            var aggregateName = useCase.Namespace!.Split('.')[2];

            // Use case
            Check(
                errors,
                useCase,
                $"{projectName}.Application.{aggregateName}.{path}");

            // Handler
            var handler = GetHandler(useCase, types);

            if (handler == null)
            {
                errors.Add($"{useCase.FullName}: use case must be have handler");
            }
            else
            {
                if (isCommand)
                {
                    Check(
                        errors,
                        handler,
                        $"{projectName}.Application.{aggregateName}.Commands");
                }
                else
                {
                    Check(
                        errors,
                        handler,
                        $"{projectName}.Application.{aggregateName}.Queries",
                        $"{projectName}.Infrastructure.{aggregateName}.Queries");
                }
            }

            // Validator
            var validator = GetValidator(useCase, types);

            if (validator != null)
            {
                Check(
                    errors,
                    validator,
                    $"{projectName}.Infrastructure.{aggregateName}.Validators.{path}");
            }

            // Endpoint
            var endpoint = GetEndpoint(useCase, types);

            if (endpoint != null)
            {
                Check(
                    errors,
                    endpoint,
                    $"{projectName}.Api.{aggregateName}.{path}");
            }
        }

        return errors;
    }
}