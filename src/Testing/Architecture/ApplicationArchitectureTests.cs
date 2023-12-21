using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Queries;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class ApplicationArchitectureTests
{
    protected abstract Assembly GetApplicationAssembly();

    protected abstract Assembly GetInfrastructureAssembly();

    [Fact]
    public void Queries_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .ImplementInterface(typeof(IQueryRequest<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssemblies(new[] { GetApplicationAssembly(), GetInfrastructureAssembly() })
            .That()
            .ImplementInterface(typeof(IQueryRequestHandler<,>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .ImplementInterface(typeof(ICommandRequest))
            .Or()
            .ImplementInterface(typeof(ICommandRequest<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandsHandlers_Should_BeSealed_And_NotBePublic()
    {
        // Act
        var result = Types.InAssembly(GetApplicationAssembly())
            .That()
            .ImplementInterface(typeof(ICommandRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandRequestHandler<,>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
