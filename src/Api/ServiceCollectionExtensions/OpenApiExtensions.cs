using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;
// ReSharper disable CommentTypo

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    /// <summary>
    /// Open api options
    /// </summary>
    public class OpenApiOptions
    {
        /// <summary>
        /// Open api info title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Open api name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// De Url of swagger.json. Default: "swagger/v1/swagger.json"
        /// </summary>
        public string Url { get; set; } = "swagger/v1/swagger.json";

        /// <summary>
        /// Documentation file name
        /// </summary>
        public string XmlDocumentationFile { get; set; }
    }

    /// <summary>
    /// Swagger configuration
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Services configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernelOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApiOptions = new OpenApiOptions();
            configuration.GetSection(nameof(OpenApiOptions)).Bind(openApiOptions);

            services.Configure<OpenApiOptions>(configuration.GetSection(nameof(OpenApiOptions)));

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddFluentValidationRulesToSwagger();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = openApiOptions.Title, Version = "v1" });

                c.DescribeAllParametersInCamelCase();

                // Set the comments path for the Swagger JSON and UI.
                string xmlPath = null;
                if (!string.IsNullOrWhiteSpace(openApiOptions.XmlDocumentationFile))
                {
                    xmlPath = Path.Combine(AppContext.BaseDirectory, openApiOptions.XmlDocumentationFile);
                    c.IncludeXmlComments(xmlPath);
                }

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
                                Scopes = openIdOptions.Scopes.ToDictionary(s => s.Name, s => s.DisplayName)
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

                    // get descriptions from xml-file comments on the specified path should use "options.IncludeXmlComments(xmlFilePath);" before
                    if (!string.IsNullOrWhiteSpace(xmlPath))
                        o.IncludeXmlCommentsFrom(xmlPath);
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        /// <summary>
        /// Configure Open Api ui
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <param name="openIdOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSharedKernelOpenApi(this IApplicationBuilder app,
            IOptions<OpenApiOptions> options, IOptions<OpenIdOptions> openIdOptions)
        {
            if (options.Value == default)
                throw new ArgumentNullException(nameof(options));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(options.Value.Url, options.Value?.Name ?? "Open API v1");

                if (string.IsNullOrWhiteSpace(openIdOptions?.Value?.Authority))
                    return;

                c.RoutePrefix = string.Empty;
                c.OAuthAppName(options.Value?.AppName ?? "Open API specification");
                c.OAuthScopeSeparator(" ");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                c.OAuthClientId(openIdOptions.Value.ClientId);
                c.OAuthClientSecret(openIdOptions.Value.ClientSecret);
            });

            return app;
        }

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
    }
}
