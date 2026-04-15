using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using SharedKernel.Application.Security;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi;

/// <summary> SwaggerGenOptions Extensions. </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Add one or more "securityDefinitions", describing how your API is protected, to the generated Swagger
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="openIdOptions"><see cref="OpenIdOptions"/></param>
    /// <param name="openApiOptions"><see cref="OpenApiOptions"/></param>
    public static void AddSecurityDefinition(this SwaggerGenOptions swaggerGenOptions, OpenIdOptions openIdOptions,
        OpenApiOptions openApiOptions)
    {
        var authority = openApiOptions.Authority ?? openIdOptions.Authority;
        var tokenEndpoint = openApiOptions.TokenEndpoint ?? openIdOptions.TokenEndpoint;
        if (string.IsNullOrWhiteSpace(authority))
            return;

        swaggerGenOptions.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                Password = new OpenApiOAuthFlow
                {
                    //AuthorizationUrl = new Uri(authority),
                    TokenUrl = new Uri($"{authority}/{tokenEndpoint}"),
                    Scopes = openIdOptions.Scopes.ToDictionary(s => s.Name, s => s.DisplayName),
                },
            },
        });
    }

    /// <summary>
    /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="openApiOptions"><see cref="OpenApiOptions"/></param>
    /// <param name="includeControllerXmlComments">
    /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
    /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
    /// </param>
    public static string? IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, OpenApiOptions openApiOptions,
        bool includeControllerXmlComments = false)
    {
        string? xmlPath = default!;
        if (openApiOptions.XmlDocumentationFiles.Any())
        {
            xmlPath = swaggerGenOptions.IncludeXmlComments(openApiOptions.XmlDocumentationFiles, includeControllerXmlComments);
        }
        else if (!string.IsNullOrWhiteSpace(openApiOptions.XmlDocumentationFile))
        {
            xmlPath = Path.Combine(AppContext.BaseDirectory, openApiOptions.XmlDocumentationFile);
            swaggerGenOptions.IncludeXmlComments(xmlPath, includeControllerXmlComments);
        }

        return xmlPath;
    }

    /// <summary>
    /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="filePaths">An absolute path to the file that contains XML Comments</param>
    /// <param name="includeControllerXmlComments">
    /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
    /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
    /// </param>
    public static string? IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, IEnumerable<string> filePaths, bool includeControllerXmlComments = false)
    {
        var filesList = filePaths.ToList();

        if (!filesList.Any())
            return default;

        XElement? xml = default;
        foreach (var qualifiedFileName in filesList.Select(fileName => Path.Combine(AppContext.BaseDirectory, fileName)))
        {
            if (xml == default)
            {
                xml = XElement.Load(qualifiedFileName);
            }
            else
            {
                var dependentXml = XElement.Load(qualifiedFileName);
                foreach (var ele in dependentXml.Descendants())
                    xml.Add(ele);
            }
        }

        if (xml == default)
            return default;

        var swaggerFile = $"{AppContext.BaseDirectory}/SwaggerFile.xml";
        xml.Save(swaggerFile);
        swaggerGenOptions.IncludeXmlComments(swaggerFile, includeControllerXmlComments);

        return swaggerFile;
    }
}
