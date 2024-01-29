using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Queries;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class ApplicationArchitectureTests : BaseArchitectureTest
{
    protected abstract Assembly GetApplicationAssembly();

    protected abstract Assembly GetInfrastructureAssembly();

    [Fact]
    public void Queries_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(IQueryRequest<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssemblies(new[] { GetApplicationAssembly(), GetInfrastructureAssembly() })
            .BeSealedAndNotPublicEndingWith(typeof(IQueryRequestHandler<,>), "Handler");

        // Assert
        Assert(result);
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(ICommandRequest))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void Commands_Should_BeSealed_Result()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(ICommandRequest<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void CommandsHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(ICommandRequestHandler<>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith("Handler")
            .GetResult();

        // Assert
        Assert(result);
    }

    [Fact]
    public void CommandsHandlersResult_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(ICommandRequestHandler<,>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith("Handler")
            .GetResult();

        // Assert
        Assert(result);
    }
}
