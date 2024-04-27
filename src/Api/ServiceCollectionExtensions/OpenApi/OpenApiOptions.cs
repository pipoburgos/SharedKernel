namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi;

/// <summary> Open api options./// </summary>
public class OpenApiOptions
{
    /// <summary> Open api info title. </summary>
    public string Title { get; set; } = null!;

    /// <summary> Application name. </summary>
    public string? AppName { get; set; }

    /// <summary> Open api name. </summary>
    public string? Name { get; set; }

    /// <summary> TokenEndpoint. </summary>
    public string? TokenEndpoint { get; set; } = "connect/token";

    /// <summary> De Url of swagger.json. Default: "swagger/v1/swagger.json". </summary>
    public string Url { get; set; } = "swagger/v1/swagger.json";

    /// <summary> De Url of api. </summary>
    public string? UrlApi { get; set; }

    /// <summary> Documentation file name. </summary>
    public string? XmlDocumentationFile { get; set; }

    /// <summary> Collapse actions of controllers. </summary>
    public bool Collapsed { get; set; } = true;

    /// <summary> Authority. </summary>
    public string? Authority { get; set; }

    /// <summary> Documentation files names. </summary>
    public IEnumerable<string> XmlDocumentationFiles { get; set; } = new List<string>();
}
