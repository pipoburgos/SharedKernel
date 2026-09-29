using FluentValidation;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Requests;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public static class CqrsNamespaceArchitecture
{
    public static List<string> TestCqrsTypesShouldBeInSameNamespace(this IEnumerable<Assembly> assemblies)
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

        foreach (var useCase in useCases)
        {
            if (IsCommand(useCase))
                ValidateCommand(errors, types, useCase);
            else if (IsQuery(useCase))
                ValidateQuery(errors, types, useCase);
            else
                errors.Add($"{useCase.FullName}: use case must be a command or a query");
        }

        return errors;
    }

    // ============================================================
    // COMMANDS
    // ============================================================

    private static void ValidateCommand(List<string> errors, List<Type> types, Type command)
    {
        ValidateApplicationNamespace(errors, command);
        ValidateCommandHandler(errors, types, command);
        ValidateCommandValidator(errors, types, command);
        ValidateCommandResponseValidators(errors, types, command);
        ValidateEndpoint(errors, types, command);
    }

    private static void ValidateCommandHandler(List<string> errors, List<Type> types, Type command)
    {
        var handlerName = $"{command.Name}Handler";

        var handlers = types
            .Where(t => t.Name == handlerName)
            .ToList();

        if (handlers.Count == 0)
        {
            //errors.Add($"{command.FullName}: handler '{handlerName}' not found");

            return;
        }

        if (handlers.Count > 1)
        {
            //errors.Add(
            //    $"{command.FullName}: multiple handlers named '{handlerName}' found: " +
            //    $"{string.Join(", ", handlers.Select(x => x.FullName))}");

            return;
        }

        var handler = handlers[0];

        ValidateHandlerInterface(errors, command, handler);

        // CommandHandler debe estar exactamente en el mismo namespace.
        if (handler.Namespace != command.Namespace)
        {
            errors.Add(
                $"{handler.FullName}: command handler must be in the same namespace " +
                $"as {command.FullName}");
        }
    }

    private static void ValidateCommandResponseValidators(List<string> errors, List<Type> types, Type query)
    {
        var queryInterface = GetCommandInterface(query);

        if (queryInterface == null)
            return;

        var responseType = queryInterface.GetGenericArguments()[0];

        ValidateResponseType(errors, types, query, responseType);
    }

    private static void ValidateCommandValidator(List<string> errors, List<Type> types, Type command)
    {
        var validatorName = $"{command.Name}Validator";

        var validators = types
            .Where(t => t.Name == validatorName)
            .ToList();

        if (validators.Count == 0)
        {
            //errors.Add($"{command.FullName}: validator '{validatorName}' not found");

            return;
        }

        if (validators.Count > 1)
        {
            //errors.Add(
            //    $"{command.FullName}: multiple validators named '{validatorName}' found: " +
            //    $"{string.Join(", ", validators.Select(x => x.FullName))}");

            return;
        }

        var validator = validators[0];

        ValidateValidatorInterface(errors, command, validator);
        var expectedNamespace = GetExpectedValidatorNamespace(command);

        ValidateNamespace(errors, validator, expectedNamespace, "command validator");
    }

    // ============================================================
    // QUERIES
    // ============================================================

    private static void ValidateQuery(List<string> errors, List<Type> types, Type query)
    {
        ValidateApplicationNamespace(errors, query);
        ValidateQueryHandler(errors, types, query);
        ValidateQueryValidator(errors, types, query);
        ValidateQueryResponseValidators(errors, types, query);
        ValidateEndpoint(errors, types, query);
    }

    private static void ValidateQueryHandler(List<string> errors, List<Type> types, Type query)
    {
        var handlerName = $"{query.Name}Handler";

        var handlers = types
            .Where(t => t.Name == handlerName)
            .ToList();

        if (handlers.Count == 0)
        {
            //errors.Add($"{query.FullName}: handler '{handlerName}' not found");

            return;
        }

        if (handlers.Count > 1)
        {
            //errors.Add(
            //    $"{query.FullName}: multiple handlers named '{handlerName}' found: " +
            //    $"{string.Join(", ", handlers.Select(x => x.FullName))}");

            return;
        }

        var handler = handlers[0];

        ValidateHandlerInterface(errors, query, handler);

        var applicationNamespace = query.Namespace;

        if (applicationNamespace == null)
            return;

        var infrastructureNamespace = ToInfrastructureNamespace(applicationNamespace);

        // QueryHandler puede estar en Application o Infrastructure.
        var validNamespaces = new[]
        {
            applicationNamespace,
            infrastructureNamespace,
        };

        if (!validNamespaces.Contains(handler.Namespace))
        {
            errors.Add(
                $"{handler.FullName}: query handler must be in one of: " +
                $"{string.Join(", ", validNamespaces)}");
        }
    }

    private static void ValidateQueryValidator(List<string> errors, List<Type> types, Type query)
    {
        var validatorName = $"{query.Name}Validator";

        var validators = types
            .Where(t => t.Name == validatorName)
            .ToList();

        if (validators.Count == 0)
        {
            //errors.Add($"{query.FullName}: validator '{validatorName}' not found");

            return;
        }

        if (validators.Count > 1)
        {
            //errors.Add(
            //    $"{query.FullName}: multiple validators named '{validatorName}' found: " +
            //    $"{string.Join(", ", validators.Select(x => x.FullName))}");

            return;
        }

        var validator = validators[0];

        ValidateValidatorInterface(errors, query, validator);

        var expectedNamespace = GetExpectedValidatorNamespace(query);

        ValidateNamespace(errors, validator, expectedNamespace, "query validator");
    }

    // ============================================================
    // QUERY RESPONSE VALIDATORS
    // ============================================================

    private static void ValidateQueryResponseValidators(List<string> errors, List<Type> types, Type query)
    {
        var queryInterface = GetQueryInterface(query);

        if (queryInterface == null)
            return;

        var responseType = queryInterface.GetGenericArguments()[0];

        ValidateResponseType(errors, types, query, responseType);
    }

    private static void ValidateResponseType(
        List<string> errors,
        List<Type> types,
        Type query,
        Type type)
    {
        // Recorremos los argumentos genéricos.
        //
        // Ejemplo:
        // IPagedList<BankAccountItem>
        //
        // llega hasta:
        // BankAccountItem

        if (type.IsGenericType)
        {
            foreach (var argument in type.GetGenericArguments())
            {
                ValidateResponseType(
                    errors,
                    types,
                    query,
                    argument);
            }

            return;
        }

        // Tipos simples no necesitan validator.
        if (!RequiresValidator(type))
            return;

        var validatorType = typeof(IValidator<>)
            .MakeGenericType(type);

        var validators = types
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                validatorType.IsAssignableFrom(t))
            .ToList();

        if (validators.Count == 0)
        {
            //errors.Add(
            //    $"{query.FullName}: response type '{type.FullName}' " +
            //    $"requires validator '{type.Name}Validator'");

            return;
        }

        if (validators.Count > 1)
        {
            //errors.Add(
            //    $"{query.FullName}: multiple validators found for response type " +
            //    $"'{type.FullName}': " +
            //    $"{string.Join(", ", validators.Select(x => x.FullName))}");

            return;
        }

        var validator = validators[0];

        var expectedNamespace = GetExpectedValidatorNamespace(query);

        ValidateNamespace(
            errors,
            validator,
            expectedNamespace,
            $"query response validator for {type.Name}");
    }

    // ============================================================
    // COMMON VALIDATION
    // ============================================================

    private static void ValidateApplicationNamespace(
        List<string> errors,
        Type type)
    {
        if (string.IsNullOrWhiteSpace(type.Namespace))
        {
            errors.Add($"{type.FullName}: namespace is missing");

            return;
        }

        if (!type.Namespace.Contains(
                ".Application.",
                StringComparison.Ordinal))
        {
            errors.Add($"{type.FullName}: CQRS request must be in an Application namespace");
        }

        if (!type.Namespace.Contains(
                ".Commands",
                StringComparison.Ordinal) &&
            !type.Namespace.Contains(
                ".Queries",
                StringComparison.Ordinal))
        {
            errors.Add($"{type.FullName}: namespace must contain '.Commands.' or '.Queries.'");
        }
    }

    private static void ValidateHandlerInterface(
        List<string> errors,
        Type useCase,
        Type handler)
    {
        var isHandler = handler
            .GetInterfaces()
            .Any(@interface =>
                @interface.IsGenericType &&
                @interface.GetGenericArguments()[0] == useCase &&
                IsHandlerInterface(@interface));

        if (!isHandler)
        {
            errors.Add(
                $"{handler.FullName}: handler does not implement a valid handler " +
                $"for {useCase.FullName}");
        }
    }

    private static void ValidateValidatorInterface(
        List<string> errors,
        Type request,
        Type validator)
    {
        var isValidator = validator
            .GetInterfaces()
            .Any(@interface =>
                @interface.IsGenericType &&
                @interface.GetGenericTypeDefinition() ==
                typeof(IValidator<>) &&
                @interface.GetGenericArguments()[0] == request);

        if (!isValidator)
        {
            errors.Add(
                $"{validator.FullName}: validator does not implement " +
                $"IValidator<{request.Name}>");
        }
    }

    private static void ValidateNamespace(
        List<string> errors,
        Type type,
        string? expectedNamespace,
        string description)
    {
        if (expectedNamespace == null)
            return;

        if (type.Namespace != expectedNamespace)
        {
            errors.Add(
                $"{type.FullName}: {description} must be in " +
                $"{expectedNamespace}");
        }
    }

    // ============================================================
    // NAMESPACE RESOLUTION
    // ============================================================

    private static string GetExpectedValidatorNamespace(Type useCase)
    {
        var applicationNamespace = useCase.Namespace!;

        const string applicationMarker = ".Application.";

        var applicationIndex = applicationNamespace.IndexOf(
            applicationMarker,
            StringComparison.Ordinal);

        if (applicationIndex < 0)
            throw new InvalidOperationException(
                $"Cannot determine Application namespace for '{useCase.FullName}'.");

        var rootNamespace = applicationNamespace[..applicationIndex];

        var featureNamespace = applicationNamespace[
            (applicationIndex + applicationMarker.Length)..];

        var commandsIndex = featureNamespace.IndexOf(
            ".Commands",
            StringComparison.Ordinal);

        var queriesIndex = featureNamespace.IndexOf(
            ".Queries",
            StringComparison.Ordinal);

        if (commandsIndex >= 0)
        {
            var feature = featureNamespace[..commandsIndex];

            return $"{rootNamespace}.Infrastructure.{feature}.Validators.Commands";
        }

        if (queriesIndex >= 0)
        {
            var feature = featureNamespace[..queriesIndex];

            return $"{rootNamespace}.Infrastructure.{feature}.Validators.Queries";
        }

        throw new InvalidOperationException(
            $"Cannot determine Commands/Queries namespace for '{useCase.FullName}'.");
    }

    private static string ToInfrastructureNamespace(
        string applicationNamespace)
    {
        return applicationNamespace.Replace(
            ".Application.",
            ".Infrastructure.",
            StringComparison.Ordinal);
    }

    // ============================================================
    // TYPE DETECTION
    // ============================================================

    private static bool IsUseCase(Type type)
    {
        return typeof(IRequest).IsAssignableFrom(type) &&
               !typeof(DomainEvent).IsAssignableFrom(type);
    }

    private static bool IsCommand(Type type)
    {
        return type.GetInterfaces()
            .Any(x =>
                x == typeof(ICommandRequest) ||
                (
                    x.IsGenericType &&
                    (x.GetGenericTypeDefinition() == typeof(ICommandRequest) ||
                     x.GetGenericTypeDefinition() == typeof(ICommandRequest<>))
                ));
    }

    private static bool IsQuery(Type type)
    {
        return GetQueryInterface(type) != null;
    }

    private static Type? GetCommandInterface(Type type)
    {
        return type
            .GetInterfaces()
            .FirstOrDefault(x =>
                x.IsGenericType &&
                (x.GetGenericTypeDefinition() == typeof(ICommandRequest) ||
                 x.GetGenericTypeDefinition() == typeof(ICommandRequest<>))
            );
    }

    private static Type? GetQueryInterface(Type type)
    {
        return type
            .GetInterfaces()
            .FirstOrDefault(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryRequest<>));
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genericType = type.GetGenericTypeDefinition();

        return genericType == typeof(ICommandRequestHandler<>) ||
               genericType == typeof(ICommandRequestHandler<,>) ||
               genericType == typeof(IQueryRequestHandler<,>);
    }

    private static bool RequiresValidator(Type type)
    {
        if (type == typeof(string))
            return false;

        if (type == typeof(object))
            return false;

        // decimal, int, Guid, DateTime, bool, etc.
        if (!type.IsClass)
            return false;

        return true;
    }

    // ============================================================
    // REFLECTION
    // ============================================================

    private static IEnumerable<Type> GetLoadableTypes(
        Assembly assembly)
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

    private static void ValidateEndpoint(
        List<string> errors,
        List<Type> types,
        Type useCase)
    {
        var expectedNamespace = GetExpectedEndpointNamespace(useCase);
        var expectedName = $"{useCase.Name}Endpoint";

        var endpoints = types
            .Where(x =>
                x.Name == expectedName &&
                string.Equals(
                    x.Namespace,
                    expectedNamespace,
                    StringComparison.Ordinal))
            .ToList();

        if (endpoints.Count == 0)
        {
            //errors.Add(
            //    $"{useCase.FullName}: endpoint '{expectedNamespace}.{expectedName}' not found.");
        }
        else if (endpoints.Count > 1)
        {
            errors.Add(
                $"{useCase.FullName}: multiple endpoints '{expectedNamespace}.{expectedName}' found.");
        }
    }

    private static string GetExpectedEndpointNamespace(Type useCase)
    {
        var applicationNamespace = useCase.Namespace!;

        const string applicationMarker = ".Application.";

        var applicationIndex = applicationNamespace.IndexOf(
            applicationMarker,
            StringComparison.Ordinal);

        if (applicationIndex < 0)
        {
            throw new InvalidOperationException(
                $"Cannot determine Application namespace for '{useCase.FullName}'.");
        }

        var rootNamespace = applicationNamespace[..applicationIndex];

        var featureNamespace = applicationNamespace[
            (applicationIndex + applicationMarker.Length)..];

        return $"{rootNamespace}.Api.{featureNamespace}";
    }
}