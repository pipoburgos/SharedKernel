using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.DocumentFilters;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi;

/// <summary> Swagger configuration. </summary>
public static class OpenApiExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelMicrosoftOpenApi(this IServiceCollection services,
        int defaultVersion = 1, string[]? versions = null)
    {
        versions ??= ["v1"];

        foreach (var version in versions)
        {
            services.AddOpenApi(version, o => o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1);
        }

        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(defaultVersion, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    private sealed class ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider,
        IOptions<OpenApiOptions> openApiOptions)
        : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var version = $"v{description.ApiVersion.MajorVersion}";

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"{openApiOptions.Value.Title} {version}",
                    Version = version,
                });
            }
        }
    }

    /// <summary> Services configuration. </summary>
    public static IServiceCollection AddSharedKernelSwashbuckle(this IServiceCollection services, IConfiguration configuration, Action<SwaggerGenOptions>? setupAction = null)
    {
        var openApiOptions = new OpenApiOptions();
        configuration.GetSection(nameof(OpenApiOptions)).Bind(openApiOptions);
        services.Configure<OpenApiOptions>(configuration.GetSection(nameof(OpenApiOptions)));

        var openIdOptions = new OpenIdOptions();
        configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

        services.AddSwaggerGen(swaggerGenOptions =>
        {
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

#if NET6_0_OR_GREATER
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
#else
                var basePath = AppDomain.CurrentDomain.BaseDirectory
                    ?? throw new NullReferenceException(nameof(AppDomain.CurrentDomain.BaseDirectory));
#endif

            foreach (var xmlDocumentationFile in openApiOptions.XmlDocumentationFiles)
            {
                swaggerGenOptions.IncludeXmlComments(Path.Combine(basePath, xmlDocumentationFile), true);
            }

            if (!string.IsNullOrWhiteSpace(openIdOptions.Authority))
            {
                swaggerGenOptions.AddSecurityDefinition(openIdOptions, openApiOptions);

                swaggerGenOptions.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("oauth2", document)] = [],
                });
                swaggerGenOptions.OperationFilter<SecurityAllAuthorizeExceptAllowAnonymousOperationFilter>();
            }

            swaggerGenOptions.SupportNonNullableReferenceTypes();
            swaggerGenOptions.DescribeAllParametersInCamelCase();

            // DocumentFilters
            swaggerGenOptions.DocumentFilter<TagReOrderDocumentFilter>();

            // OperationFilters
            swaggerGenOptions.OperationFilter<RequiredQueryParametersOperationFilter>();

            // SchemaFilters
            swaggerGenOptions.SchemaFilter<HideNonPublicCommandPropertiesSchemaFilter>();
            swaggerGenOptions.SchemaFilter<RequiredFromConstructorSchemaFilter>();
            //swaggerGenOptions.SchemaFilter<AssignPropertyRequiredSchemaFilter>();
            swaggerGenOptions.SchemaFilter<XEnumNamesSchemaFilter>(
                openApiOptions.XmlDocumentationFiles.Select(xmlDocumentationFile =>
                    Path.Combine(basePath, xmlDocumentationFile)));

            // OperationFilters
            //swaggerGenOptions.OperationFilter<FromQueryModelOperationFilter>();
            //swaggerGenOptions.OperationFilter<OptionalParameterOperationFilter>();

            //swaggerGenOptions.CustomOperationIds(_ => default);

            setupAction?.Invoke(swaggerGenOptions);
        });

        return services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    /// <summary> Configure Open Api UI. </summary>
    public static IApplicationBuilder UseSharedKernelSwashbuckle(this IApplicationBuilder app,
        Action<SwaggerUIOptions>? setupActionUi = null, Action<SwaggerOptions>? setupAction = null)
    {
        var openApiOptions = app.ApplicationServices.GetRequiredService<IOptions<OpenApiOptions>>().Value;
        var openIdOptions = app.ApplicationServices.GetService<IOptions<OpenIdOptions>>();

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}.json";

            setupAction?.Invoke(c);

            c.PreSerializeFilters.Add((swaggerDoc, _) =>
                swaggerDoc.Servers = openApiOptions.ServersUrls.Select(url => new OpenApiServer { Url = url }).ToList());
        });

        app.UseSwaggerUI(c =>
        {
            if (openApiOptions.Collapsed)
                c.DocExpansion(DocExpansion.None);

            setupActionUi?.Invoke(c);

            c.RoutePrefix = string.Empty;
            c.OAuthAppName(openApiOptions.AppName ?? "Open API specification");
            c.OAuthScopeSeparator(" ");
            c.OAuthUseBasicAuthenticationWithAccessCodeGrant();

            if (openIdOptions?.Value.ClientId == default!)
                return;

            c.OAuthClientId(openIdOptions.Value.ClientId);
            if (openIdOptions.Value.ClientSecret != default!)
                c.OAuthClientSecret(openIdOptions.Value.ClientSecret);
        });

        return app;
    }
}
