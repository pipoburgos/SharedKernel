using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Domain.Repositories;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class ArchitectureTests
{
    protected abstract Assembly GetDomainAssembly();

    protected abstract Assembly GetApplicationAssembly();

    protected abstract Assembly GetInfrastructureAssembly();

    protected abstract Assembly GetApiAssembly();

    protected abstract Assembly GetUseCasesTestsAssembly();

    protected abstract Assembly GetAcceptanceTestsAssembly();

    protected abstract List<CheckFile> CheckFiles();

    protected virtual bool CheckQueryValidators => false;

    protected abstract void Assert(TestResult? testResult);

    protected abstract void Assert(List<Type>? failingTypes);

    protected abstract void Assert(List<string> files);

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Assert(GetDomainAssembly().TestDomainEventsShouldBeSealed());
    }

    [Fact]
    public void Entities_ShouldNot_HavePublicConstructors()
    {
        Assert(GetDomainAssembly().TestEntitiesShouldNotHavePublicConstructors());
    }

    [Fact]
    public void Entities_Should_HavePublicFactory()
    {
        Assert(GetDomainAssembly().TestEntitiesShouldHavePublicFactory());
    }

    [Fact]
    public void Queries_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(IQueryRequest<>));

        // Assert
        Assert(result);
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssemblies(new[] { GetApplicationAssembly(), GetInfrastructureAssembly() })
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IQueryRequestHandler<,>), "Handler");

        // Assert
        Assert(result);
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest));

        // Assert
        Assert(result);
    }

    [Fact]
    public void Commands_Should_BeSealed_Result()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest<>));

        // Assert
        Assert(result);
    }

    [Fact]
    public void CommandsHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<>), "Handler");

        // Assert
        Assert(result);
    }

    [Fact]
    public void CommandsHandlersResult_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<,>), "Handler");

        // Assert
        Assert(result);
    }

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

    [Fact]
    public void Endpoints_Should_BeSealed_And_HaveNameEndingWithEndpoint()
    {
        // Arrange and Act
        var result = Types.InAssembly(GetApiAssembly())
            .That()
            .Inherit(typeof(EndpointBase))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .HaveNameEndingWith("Endpoint")
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void TestCqrs()
    {
        // Arrange and Act
        var files = new List<Assembly>
        {
            GetApplicationAssembly(),
            GetInfrastructureAssembly(),
            GetApiAssembly(),
            GetUseCasesTestsAssembly(),
            GetAcceptanceTestsAssembly()
        }.TestCqrsArquitecture(CheckFiles(), CheckQueryValidators);

        // Assert
        Assert(files);
    }
}
