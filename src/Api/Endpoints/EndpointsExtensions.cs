using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Api.Endpoints;

public static class EndpointsExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelEndpoints(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSharedKernelFromMatchingInterface<IEndpoint>(ServiceLifetime.Scoped, assembly);
    }

    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        using var scope = app.ServiceProvider.CreateScope();

        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
    }

    public static IResult ToIResult(this Result<Unit> result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(new ValidationError(new ValidationFailureException(result.Errors
            .Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage)).ToList())));
    }
}