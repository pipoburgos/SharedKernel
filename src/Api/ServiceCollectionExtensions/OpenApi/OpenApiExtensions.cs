using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.DocumentFilters;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi
{
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

            var openIdOptions = new OpenIdOptions();
            configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddFluentValidationRulesToSwagger();

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = openApiOptions.Title, Version = "v1" });

                swaggerGenOptions.OrderActionsBy(a =>
                {
                    // Sort actions in tags (controllers)
                    var order = a.HttpMethod switch
                    {
                        "GET" => 1,
                        "POST" => 2,
                        "PATCH" => 3,
                        "PUT" => 4,
                        "DELETE" => 5,
                        _ => 6
                    };
#if NET5_0
                    var relativePath = a.RelativePath;
#else
                    var relativePath = a.RelativePath ?? string.Empty;
#endif
                    var path = $"{relativePath}_{relativePath.Length.ToString().PadLeft(5, '0')}{order}";

                    return path;
                });

                swaggerGenOptions.SchemaFilter<AssignPropertyRequiredSchemaFilter>();

                swaggerGenOptions.SchemaFilter<HideNonPublicCommandPropertiesSchemaFilter>();

                swaggerGenOptions.DescribeAllParametersInCamelCase();

#if NET5_0_OR_GREATER
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
#else
                var basePath = AppDomain.CurrentDomain.BaseDirectory
                    ?? throw new NullReferenceException(nameof(AppDomain.CurrentDomain.BaseDirectory));
#endif

                foreach (var xmlDocumentationFile in openApiOptions.XmlDocumentationFiles)
                {
                    swaggerGenOptions.IncludeXmlComments(Path.Combine(basePath, xmlDocumentationFile), true);
                    swaggerGenOptions.AddEnumsWithValuesFixFilters(xmlDocumentationFile);
                }

                swaggerGenOptions.AddSecurityDefinition(openIdOptions, openApiOptions);

                if (!string.IsNullOrWhiteSpace(openIdOptions.Authority))
                    swaggerGenOptions.OperationFilter<SecurityAllAuthorizeExceptAllowAnonymousOperationFilter>();

                swaggerGenOptions.CustomOperationIds(_ => default);

                swaggerGenOptions.DocumentFilter<TagReOrderDocumentFilter>();
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

                if (options.Value?.Collapsed == true)
                    c.DocExpansion(DocExpansion.None);

                var authority = options.Value?.Authority ?? openIdOptions?.Value?.Authority;
                if (string.IsNullOrWhiteSpace(authority))
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
    }
}
