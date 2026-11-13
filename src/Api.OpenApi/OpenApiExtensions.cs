using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;

namespace SharedKernel.Api.OpenApi;

/// <summary> Swagger configuration. </summary>
public static class OpenApiExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelMicrosoftOpenApi(this IServiceCollection services,
        string[]? versions = null, Action<Microsoft.AspNetCore.OpenApi.OpenApiOptions>? configureOptions = null)
    {
        versions ??= ["v1"];

        foreach (var version in versions)
        {
            services.AddOpenApi(version, o =>
            {
                o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
                configureOptions?.Invoke(o);
            });
        }

        return services;
    }

    public static WebApplication UseSharedKernelOpenApi(this WebApplication app, [StringSyntax("Route")] string? pattern = null)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            app.MapOpenApi().AllowAnonymous();
        else
            app.MapOpenApi(pattern).AllowAnonymous();

        return app;
    }
}