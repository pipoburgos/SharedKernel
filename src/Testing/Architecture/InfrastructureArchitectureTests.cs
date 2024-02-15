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
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IBaseRepository), "Repository");

        // Assert
        Assert(result);
    }

    [Fact]
    public void Validators_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetInfrastructureAssembly())
            .ClassBeSealedAndNotPublicEndingWith(typeof(AbstractValidator<>), "Validator");

        // Assert
        Assert(result);
    }

    [Fact]
    public void Configurations_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetInfrastructureAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IEntityTypeConfiguration<>), "Configuration");

        // Assert
        Assert(result);
    }
}
