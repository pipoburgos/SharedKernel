using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.ValueObjects;
using System.Reflection;
using TestResult = NetArchTest.Rules.TestResult;

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
            ?.ToList() ?? new List<Type>();
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
