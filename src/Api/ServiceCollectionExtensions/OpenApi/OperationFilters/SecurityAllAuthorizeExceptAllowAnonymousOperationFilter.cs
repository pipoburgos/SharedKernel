using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;

/// <summary>
/// 
/// </summary>
public class SecurityAllAuthorizeExceptAllowAnonymousOperationFilter : IOperationFilter
{
    /// <summary>
    /// Apply filter
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Policy names map to scopes
        var requiredScopes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Distinct()
            .ToList();

        if (requiredScopes.Any())
            return;

        operation.Responses?.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses?.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

        var schemeReference = new OpenApiSecuritySchemeReference("oauth2", null, null);
        var securityRequirement = new OpenApiSecurityRequirement()
        {
            { schemeReference, new List<string>() },
        };

        operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
    }
}