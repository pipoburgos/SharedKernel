using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Repositories;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class InfrastructureArchitectureTests : BaseArchitectureTest
{
    protected abstract Assembly GetInfrastructureAssembly();

    [Fact]
    public void Repositories_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetInfrastructureAssembly())
            .That()
            .ImplementInterface(typeof(IBaseRepository))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith("Repository")
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void Validators_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetInfrastructureAssembly())
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith("Validator")
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void Configurations_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetInfrastructureAssembly())
            .That()
            .Inherit(typeof(IEntityTypeConfiguration<>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith("Configuration")
            .GetResult();

        // Assert
        Assert(result);
    }
}
