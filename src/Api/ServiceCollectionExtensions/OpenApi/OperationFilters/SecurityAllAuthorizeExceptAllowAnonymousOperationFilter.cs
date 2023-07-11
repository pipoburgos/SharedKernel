using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters
{
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

            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = new[] {"policy"}
                }
            };
        }
    }
}
