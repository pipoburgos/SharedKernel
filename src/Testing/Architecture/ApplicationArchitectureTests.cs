﻿using SharedKernel.Application.Cqrs.Commands;
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
            .InterfaceBeSealed(typeof(IQueryRequest<>));

        // Assert
        Assert(result);
    }

    [Fact]
    public void QueriesHandlers_Should_BeSealed_And_NotBePublic_And_HaveNameEndingWithHandler()
    {
        // Act
        var result = Types.InAssemblies([GetApplicationAssembly(), GetInfrastructureAssembly()])
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
}
