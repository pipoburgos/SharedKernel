using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    public class OpenApiOptions
    {
        public string Title { get; set; }

        public string AppName { get; set; }

        public string Name { get; set; }

        public string XmlDocumentationFile { get; set; }
    }

    /// <summary>
    /// Configuración de Swagger
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Configurar servicios
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApiOptions = new OpenApiOptions();
            configuration.GetSection(nameof(OpenApiOptions)).Bind(openApiOptions);

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = openApiOptions.Title, Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                string xmlPath = null;
                if (!string.IsNullOrWhiteSpace(openApiOptions.XmlDocumentationFile))
                {
                    xmlPath = Path.Combine(AppContext.BaseDirectory, openApiOptions.XmlDocumentationFile);
                    c.IncludeXmlComments(xmlPath);
                }

                c.AddFluentValidationRules();

                var openIdOptions = new OpenIdOptions();
                configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

                if (!string.IsNullOrWhiteSpace(openIdOptions.Authority))
                {
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(openIdOptions.Authority),
                                TokenUrl = new Uri(openIdOptions.Authority + "/connect/token"),
                                Scopes = new Dictionary<string, string> { { openIdOptions.Scope, "Scope" } }
                            }
                        }
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                            },
                            new[] { openIdOptions.Audience}
                        }
                    });
                }

                c.AddEnumsWithValuesFixFilters(services, o =>
                {
                    // add schema filter to fix enums (add 'x-enumNames' for NSwag) in schema
                    o.ApplySchemaFilter = true;

                    // add parameter filter to fix enums (add 'x-enumNames' for NSwag) in schema parameters
                    o.ApplyParameterFilter = true;

                    // add document filter to fix enums displaying in swagger document
                    o.ApplyDocumentFilter = true;

                    // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' for schema extensions) for applied filters
                    o.IncludeDescriptions = true;

                    // get descriptions from DescriptionAttribute then from xml-comments
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;

                    // get descriptions from xml-file comments on the specified path
                    // should use "options.IncludeXmlComments(xmlFilePath);" before
                    if (!string.IsNullOrWhiteSpace(xmlPath))
                        o.IncludeXmlCommentsFrom(xmlPath);
                    // the same for another xml-files...
                });



                //c.OperationFilter<RemoveVersionFromParameter>();
                //c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                //c.OperationFilter<FileOperation>();

                // remove Paths and Defenitions from OpenApi documentation without accepted roles
                // c.DocumentFilter<HidePathsAndDefinitionsByRolesDocumentFilter>(new List<string> { "AcceptedRole" });
            });

            return services;
        }

        /// <summary>
        /// Configurar aplicación
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app, IOptions<OpenApiOptions> options)
        {
            app.UseSwagger(opt => { });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", options?.Value?.Name ?? "Open API v1");
                c.RoutePrefix = string.Empty;
                c.OAuthAppName(options?.Value?.AppName ?? "Open API specification");
                c.OAuthScopeSeparator(" ");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            return app;
        }

        //private class RemoveVersionFromParameter : IOperationFilter
        //{
        //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //    {
        //        var versionParameter = operation.Parameters.Single(p => p.Name == "version");
        //        operation.Parameters.Remove(versionParameter);
        //    }
        //}

        //private class ReplaceVersionWithExactValueInPath : IDocumentFilter
        //{
        //    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        //    {
        //        //swaggerDoc.Paths = swaggerDoc.Paths
        //        //    .ToDictionary(
        //        //        path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
        //        //        path => path.Value
        //        //    );
        //    }
        //}

        private class SecurityRequirementsOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                // Policy names map to scopes
                var requiredScopes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Select(attr => attr.Policy)
                    .Distinct()
                    .ToList();

                if (!requiredScopes.Any()) return;

                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                //operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                //{
                //    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] {"demo_api"}}}
                //};

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = new[] {"demo_api"}
                    }
                };
            }
        }

        //private class FileOperation : IOperationFilter
        //{

        //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //    {
        //        if (operation.OperationId.ToLower() == "apifileget")
        //        {
        //            operation.Produces = new[] { "application/octet-stream" };
        //            operation.Responses["200"].Schema = new Schema { Type = "file", Description = "Download file" };
        //        }
        //    }
        //}
    }
}
