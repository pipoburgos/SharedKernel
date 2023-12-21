using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.ValueObjects;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class DomainArchitectureTests
{
    protected abstract Assembly GetDomainAssembly();

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetDomainAssembly())
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldNot_HavePublicConstructors()
    {
        var entities = Types.InAssembly(GetDomainAssembly())
            .That()
            .Inherit(typeof(AggregateRoot<>))
            .Or()
            .Inherit(typeof(Entity<>))
            .Or()
            .Inherit(typeof(ValueObject<>))
            .GetTypes();

        var failingTypes = entities
            .Where(entity => entity.GetConstructors().Any(c => c.IsPublic));

        failingTypes.Should().BeEmpty();
    }


    [Fact]
    public void Entities_Should_HavePublicFactory()
    {
        var entities = Types.InAssembly(GetDomainAssembly())
            .That()
            .Inherit(typeof(AggregateRoot<>))
            .Or()
            .Inherit(typeof(Entity<>))
            .Or()
            .Inherit(typeof(ValueObject<>))
            .GetTypes();

        var failingTypes = entities
            .Where(entity => entity.GetMethods(BindingFlags.Public | BindingFlags.Static).All(c => c.Name != "Create"));

        failingTypes.Should().BeEmpty();
    }
}
