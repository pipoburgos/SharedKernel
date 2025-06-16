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

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi;

/// <summary> Swagger configuration. </summary>
public static class OpenApiExtensions
{
    /// <summary> Services configuration. </summary>
    public static IServiceCollection AddSharedKernelOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        var openApiOptions = new OpenApiOptions();
        configuration.GetSection(nameof(OpenApiOptions)).Bind(openApiOptions);
        services.Configure<OpenApiOptions>(configuration.GetSection(nameof(OpenApiOptions)));

        var openIdOptions = new OpenIdOptions();
        configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

        return services.AddSwaggerGen(swaggerGenOptions =>
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
                    _ => 6,
                };
                var relativePath = a.RelativePath ?? string.Empty;
                var path = $"{relativePath}_{relativePath.Length.ToString().PadLeft(5, '0')}{order}";

                return path;
            });

            swaggerGenOptions.SchemaFilter<HideNonPublicCommandPropertiesSchemaFilter>();

            // Not working
            //swaggerGenOptions.SupportNonNullableReferenceTypes();

            swaggerGenOptions.SchemaFilter<AssignPropertyRequiredSchemaFilter>();

            swaggerGenOptions.DescribeAllParametersInCamelCase();

#if NET6_0_OR_GREATER
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

            //swaggerGenOptions.CustomOperationIds(_ => default);

            swaggerGenOptions.DocumentFilter<TagReOrderDocumentFilter>();
        });
    }

    /// <summary> Configure Open Api UI. </summary>
    public static IApplicationBuilder UseSharedKernelOpenApi(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<OpenApiOptions>>();
        if (options?.Value == default)
            throw new ArgumentNullException(nameof(options));

        var openIdOptions = app.ApplicationServices.GetService<IOptions<OpenIdOptions>>();

        app.UseSwagger(c =>
        {
            var url = options.Value.UrlApi;
            if (string.IsNullOrWhiteSpace(url))
                return;

            c.RouteTemplate = options.Value.RouteTemplate;
            c.PreSerializeFilters.Add((swaggerDoc, _) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = url } };
            });
        });

        app.UseSwaggerUI(c =>
        {
            var authority = options.Value.Authority ?? openIdOptions?.Value.Authority;

            var url = options.Value.Url;
            if (string.IsNullOrWhiteSpace(url))
                url = string.IsNullOrWhiteSpace(authority) ? "v1/swagger.json" : "swagger/v1/swagger.json";

            c.SwaggerEndpoint(url, options.Value.Name ?? "Open API v1");

            if (options.Value.Collapsed)
                c.DocExpansion(DocExpansion.None);

            if (string.IsNullOrWhiteSpace(authority))
                return;

            c.RoutePrefix = string.Empty;
            c.OAuthAppName(options.Value.AppName ?? "Open API specification");
            c.OAuthScopeSeparator(" ");
            c.OAuthUseBasicAuthenticationWithAccessCodeGrant();

            if (openIdOptions?.Value.ClientId != default!)
            {
                c.OAuthClientId(openIdOptions.Value.ClientId);
                if (openIdOptions.Value.ClientSecret != default!)
                    c.OAuthClientSecret(openIdOptions.Value.ClientSecret);
            }
        });

        return app;
    }
}
