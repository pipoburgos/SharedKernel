using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Infrastructure.System;
using System.Diagnostics.CodeAnalysis;
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
        app.MapOpenApi().AllowAnonymous();

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

    /// <summary>
    /// Adds a <see cref="T:Microsoft.AspNetCore.Routing.RouteEndpoint" /> to the <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" /> that matches HTTP POST requests
    /// for the specified pattern.
    /// </summary>
    /// <param name="endpoints">The <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" /> to add the route to.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <param name="handler">The delegate executed when the endpoint is matched.</param>
    /// <returns>A <see cref="T:Microsoft.AspNetCore.Builder.RouteHandlerBuilder" /> that can be used to further customize the endpoint.</returns>
    [RequiresUnreferencedCode("This API may perform reflection on the supplied delegate and its parameters. These types may be trimmed if not directly referenced.")]
    [RequiresDynamicCode("This API may perform reflection on the supplied delegate and its parameters. These types may require generated code and aren't compatible with native AOT applications.")]
    public static RouteHandlerBuilder MapQuery(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string pattern,
        Delegate handler)
    {
        //return endpoints.MapMethods(pattern, (IEnumerable<string>)[HttpMethods.Query], handler);
        return endpoints.MapMethods(pattern, [HttpMethods.Post], handler);
    }
}