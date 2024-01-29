using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.ValueObjects;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class DomainArchitectureTests : BaseArchitectureTest
{
    protected abstract Assembly GetDomainAssembly();

    protected abstract void Assert(IEnumerable<Type>? failingTypes);

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetDomainAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void Entities_ShouldNot_HavePublicConstructors()
    {
        var entities = Types.InAssembly(GetDomainAssembly())
            .That()
            .AreNotAbstract()
            .GetTypes()
            .Where(t =>
                typeof(AggregateRoot<>).IsAssignableFrom(t) ||
                typeof(Entity<>).IsAssignableFrom(t) ||
                typeof(ValueObject<>).IsAssignableFrom(t));

        var failingTypes = entities
            .Where(entity => entity.GetConstructors().Any(c => c.IsPublic));

        // Assert
        Assert(failingTypes);
    }


    [Fact]
    public void Entities_Should_HavePublicFactory()
    {
        var failingTypes = Types.InAssembly(GetDomainAssembly())
            .That()
            .AreNotAbstract()
            .GetTypes()
            .Where(t =>
                typeof(AggregateRoot<>).IsAssignableFrom(t) ||
                typeof(Entity<>).IsAssignableFrom(t) ||
                typeof(ValueObject<>).IsAssignableFrom(t))
            .Where(entity => entity.GetMethods(BindingFlags.Public | BindingFlags.Static).All(c => !c.Name.StartsWith("Create")));

        // Assert
        Assert(failingTypes);
    }
}
