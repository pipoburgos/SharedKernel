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

    // ReSharper disable once UnusedParameter.Global
    protected virtual void Assert(IReadOnlyList<string>? failingTypeNames, TestMethod? testMethod = null)
    {
        failingTypeNames?.Should().HaveCount(0);
    }

    protected virtual List<string> HandlersExclude => new();

    public enum TestMethod
    {
        DomainEventsShouldBeSealed = 1,
        EntitiesShouldNotHavePublicConstructors = 2,
        EntitiesShouldHavePublicFactory = 3,
        QueriesShouldBeSealed = 4,
        QueriesHandlersShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler = 5,
        CommandsShouldBeSealed = 6,
        CommandsShouldBeSealedResult = 7,
        CommandsHandlersShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler = 8,
        CommandsHandlersResultShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler = 9,
        CommandHandlersShouldHaveSaveChangesAndPublish = 10,
        RepositoriesShouldBeSealedAndNotBePublic = 11,
        ValidatorsShouldBeSealedAndNotBePublic = 12,
        ConfigurationsShouldBeSealedAndNotBePublic = 13,
        EndpointsShouldBeSealedAndHaveNameEndingWithEndpoint = 14,
        TestCqrsTypesShouldBeInSameNamespace = 15,
        TestCqrs = 16,

    }

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Assert(GetDomainAssembly()
            .TestDomainEventsShouldBeSealed()
            .Select(x => x.Name)
            .ToList(), TestMethod.DomainEventsShouldBeSealed);
    }

    [Fact]
    public void Entities_ShouldNot_HavePublicConstructors()
    {
        Assert(GetDomainAssembly()
            .TestEntitiesShouldNotHavePublicConstructors()
            .Select(x => x.Name)
            .ToList(), TestMethod.EntitiesShouldNotHavePublicConstructors);
    }

    [Fact]
    public void Entities_Should_HavePublicFactory()
    {
        Assert(GetDomainAssembly()
            .TestEntitiesShouldHavePublicFactory()
            .Select(x => x.Name)
            .ToList(), TestMethod.EntitiesShouldHavePublicFactory);
    }

    [Fact]
    public void Queries_Should_BeSealed()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(IQueryRequest<>))
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.QueriesShouldBeSealed);
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssemblies([GetApplicationAssembly(), GetInfrastructureAssembly()])
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IQueryRequestHandler<,>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.QueriesHandlersShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler);
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest))
            .FailingTypeNames
            ?.ToList(), TestMethod.CommandsShouldBeSealed);
    }

    [Fact]
    public void Commands_Should_BeSealed_Result()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealed(typeof(ICommandRequest<>))
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.CommandsShouldBeSealedResult);
    }

    [Fact]
    public void CommandsHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.CommandsHandlersShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler);
    }

    [Fact]
    public void CommandsHandlersResult_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        Assert(Types.InAssembly(GetApplicationAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(ICommandRequestHandler<,>), "Handler")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.CommandsHandlersResultShouldBeSealedAndNotBePublicAndHaveNameEndingWithHandler);
    }

    [Fact]
    public void CommandHandlers_Should_Have_SaveChanges_And_Publish()
    {
        Assert(GetApplicationAssembly().CommandHandlersShouldInjectEventBusAndUnitOfWork(HandlersExclude)
            .Select(x => x.Name)
            .ToList(), TestMethod.CommandHandlersShouldHaveSaveChangesAndPublish);
    }

    [Fact]
    public void Repositories_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IBaseRepository), "Repository")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.RepositoriesShouldBeSealedAndNotBePublic);
    }

    [Fact]
    public void Validators_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .ClassBeSealedAndNotPublicEndingWith(typeof(AbstractValidator<>), "Validator")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.ValidatorsShouldBeSealedAndNotBePublic);
    }

    [Fact]
    public void Configurations_Should_BeSealed_And_NotBePublic()
    {
        Assert(Types.InAssembly(GetInfrastructureAssembly())
            .InterfaceBeSealedAndNotPublicEndingWith(typeof(IEntityTypeConfiguration<>), "Configuration")
            .FailingTypes
            ?.Select(x => x.Name)
            .ToList(), TestMethod.ConfigurationsShouldBeSealedAndNotBePublic);
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
            .ToList(), TestMethod.EndpointsShouldBeSealedAndHaveNameEndingWithEndpoint);
    }

    [Fact]
    public void TestCqrsTypesShouldBeInSameNamespace()
    {
        Assert(new List<Assembly>
        {
            GetApplicationAssembly(),
            GetInfrastructureAssembly(),
            GetApiAssembly(),
            GetUseCasesTestsAssembly(),
            GetAcceptanceTestsAssembly(),
        }.TestCqrsTypesShouldBeInSameNamespace(), TestMethod.TestCqrsTypesShouldBeInSameNamespace);
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
        }.TestCqrsArquitecture(CheckFiles(), CheckQueryValidators), TestMethod.TestCqrs);
    }
}
