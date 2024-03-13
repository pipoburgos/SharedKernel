using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Requests;
using SharedKernel.Domain.ValueObjects;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public static class ArchitectureTestsExtensions
{
    public static List<Type> TestDomainEventsShouldBeSealed(this Assembly assembly)
    {
        return Types.InAssembly(assembly)
            .That()
            .AreNotAbstract()
            .And()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .And()
            .BePublic()
            .GetResult()
            .FailingTypes
            .ToList();
    }

    public static List<Type> TestEntitiesShouldNotHavePublicConstructors(this Assembly assembly)
    {
        return Types.InAssembly(assembly)
            .That()
            .AreNotAbstract()
            .GetTypes()
            .Where(t =>
                typeof(AggregateRoot<>).IsAssignableFrom(t) ||
                typeof(Entity<>).IsAssignableFrom(t) ||
                typeof(ValueObject<>).IsAssignableFrom(t))
            .Where(entity => entity.GetConstructors().Any(c => c.IsPublic))
            .ToList();
    }

    public static List<Type> TestEntitiesShouldHavePublicFactory(this Assembly assembly)
    {
        return Types.InAssembly(assembly)
            .That()
            .AreNotAbstract()
            .GetTypes()
            .Where(t =>
                typeof(AggregateRoot<>).IsAssignableFrom(t) ||
                typeof(Entity<>).IsAssignableFrom(t) ||
                typeof(ValueObject<>).IsAssignableFrom(t))
            .Where(entity => entity.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .All(c => !c.Name.StartsWith("Create")))
            .ToList();
    }

    public static List<string> TestCqrsArquitecture(this IEnumerable<Assembly> assemblies, List<CheckFile> files,
        bool checkQueryValidators = false)
    {
        var classTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        var usesCases = classTypes
            .Where(t => typeof(IRequest).IsAssignableFrom(t) && !typeof(DomainEvent).IsAssignableFrom(t))
            .ToList();

        var notFound = new List<string>();
        foreach (var useCase in usesCases)
        {
            Validate(files, notFound, classTypes, useCase, checkQueryValidators);
        }

        notFound.Should().BeEmpty();

        return notFound;
    }

    private static void Validate(IEnumerable<CheckFile> files, List<string> notFound, List<Type> classTypes,
        Type useCase, bool checkQueryValidators)
    {
        var related = classTypes
            .Select(x => x.Name)
            .Where(x => x.StartsWith(useCase.Name))
            .ToList();

        foreach (var file in files)
        {
            // Si es internal no hay que añadir los errores de endpoints
            if (!useCase.IsPublic &&
                (file == CheckFile.Endpoint ||
                 file == CheckFile.EndpointTests))
                continue;

            if ((file == CheckFile.Validator ||
                 file == CheckFile.ValidatorTests) &&
                useCase.Implements(typeof(IQueryRequest<>)) &&
                !checkQueryValidators)
                continue;

            if (related.Any(t => t.EndsWith(file.ToString("G"))))
                continue;

            notFound.Add($"{useCase.Name}{file:G}");
        }
    }

    private static bool Implements(this Type type, Type @interface)
    {
        var ok = @interface.IsAssignableFrom(type) || type.GetInterfaces().Any(x =>
            x.IsGenericType &&
            x.GetGenericTypeDefinition() == @interface);

        return ok;
    }

    public static TestResult ClassBeSealedAndNotPublicEndingWith(this Types types, Type type, string endsWith)
    {
        return types
            .That()
            .Inherit(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith(endsWith)
            .GetResult();
    }

    public static TestResult InterfaceBeSealedAndNotPublicEndingWith(this Types types, Type type, string endsWith)
    {
        return types
            .That()
            .ImplementInterface(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith(endsWith)
            .GetResult();
    }

    public static TestResult InterfaceBeSealed(this Types types, Type type)
    {
        return types
            .That()
            .ImplementInterface(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();
    }
}
