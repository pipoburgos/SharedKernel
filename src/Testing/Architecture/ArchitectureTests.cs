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

    protected virtual void Assert(List<string>? failingTypeNames)
    {
        failingTypeNames.Should().BeNullOrEmpty();
    }

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Assert(GetDomainAssembly()
            .TestDomainEventsShouldBeSealed()
            .Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Entities_ShouldNot_HavePublicConstructors()
    {
        Assert(GetDomainAssembly()
            .TestEntitiesShouldNotHavePublicConstructors()
            .Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Entities_Should_HavePublicFactory()
    {
        Assert(GetDomainAssembly()
            .TestEntitiesShouldHavePublicFactory()
            .Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Queries_Should_BeSealed()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(IQueryRequest<>))
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssemblies([GetApplicationAssembly(), GetInfrastructureAssembly()])
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IQueryRequestHandler<,>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest))
            .FailingTypeNames
            ?.ToList());
    }

    [Fact]
    public void Commands_Should_BeSealed_Result()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest<>))
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void CommandsHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void CommandsHandlersResult_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<,>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Repositories_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IBaseRepository), "Repository")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Validators_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .ClassBeSealedAndNotPublicEndingWith(typeof(AbstractValidator<>), "Validator")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Configurations_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IEntityTypeConfiguration<>), "Configuration")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void Endpoints_Should_BeSealed_And_HaveNameEndingWithEndpoint()
    {
        Assert(Types.InAssembly(GetApiAssembly())
            .That()
            .Inherit(typeof(EndpointBase))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .HaveNameEndingWith("Endpoint")
            .GetResult()
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList());
    }

    [Fact]
    public void TestCqrs()
    {
        Assert(new List<Assembly>
        {
            GetApplicationAssembly(),
            GetInfrastructureAssembly(),
            GetApiAssembly(),
            GetUseCasesTestsAssembly(),
            GetAcceptanceTestsAssembly(),
        }.TestCqrsArquitecture(CheckFiles(), CheckQueryValidators));
    }
}
