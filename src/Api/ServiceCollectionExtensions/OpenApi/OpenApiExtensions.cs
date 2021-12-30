using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

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

                swaggerGenOptions.SchemaFilter<RequireValueTypePropertiesSchemaFilter>();

                swaggerGenOptions.DescribeAllParametersInCamelCase();

                var xmlPath = swaggerGenOptions.IncludeXmlComments(openApiOptions, true);

                swaggerGenOptions.AddSecurityDefinition(openIdOptions, openApiOptions);

                swaggerGenOptions.AddEnumsWithValuesFixFilters(services, xmlPath);

                swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
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

                if (options.Value.Collapsed)
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
