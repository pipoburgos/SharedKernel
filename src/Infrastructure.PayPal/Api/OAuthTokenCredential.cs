namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// OAuthTokenCredential is used for generation of OAuth Token used by PayPal
/// REST API service. clientId and clientSecret are required by the class to
/// generate OAuth Token, the resulting token is of the form "Bearer xxxxxx". The
/// class has two constructors, one of it taking an additional Dictionary
/// used for dynamic configuration.
/// </summary>
public class OAuthTokenCredential
{
    /// <summary>
    /// Specifies the PayPal endpoint for sending an OAuth request.
    /// </summary>
    private const string OAuthTokenPath = "/v1/oauth2/token";

    /// <summary>Dynamic configuration map</summary>
    private readonly Dictionary<string, string> _config;
    /// <summary>
    /// Cached access token that is generated when calling <see cref="M:SharedKernel.Infrastructure.PayPal.Api.OAuthTokenCredential.GetAccessToken" />.
    /// </summary>
    private string _accessToken;
    /// <summary>SDKVersion instance</summary>
    private readonly SdkVersion _sdkVersion;

    /// <summary>
    /// Gets the client ID to be used when creating an OAuth token.
    /// </summary>
    public string ClientId { get; private set; }

    /// <summary>
    /// Gets the client secret to be used when creating an OAuth token.
    /// </summary>
    public string ClientSecret { get; private set; }

    /// <summary>
    /// Gets the application ID returned by OAuth servers.
    /// Must first call <see cref="M:SharedKernel.Infrastructure.PayPal.Api.OAuthTokenCredential.GetAccessToken" /> to populate this property.
    /// </summary>
    public string ApplicationId { get; private set; }

    /// <summary>
    /// Gets or sets the lifetime of a created access token in seconds.
    /// Must first call <see cref="M:SharedKernel.Infrastructure.PayPal.Api.OAuthTokenCredential.GetAccessToken" /> to populate this property.
    /// </summary>
    public int AccessTokenExpirationInSeconds { get; set; }

    /// <summary>
    /// Gets the last date when access token was generated.
    /// Must first call <see cref="M:SharedKernel.Infrastructure.PayPal.Api.OAuthTokenCredential.GetAccessToken" /> to populate this property.
    /// </summary>
    public DateTime AccessTokenLastCreationDate { get; private set; }

    /// <summary>
    /// Gets or sets the safety gap when checking the expiration of an already created access token in seconds.
    /// If the elapsed time since the last access token was created is more than the expiration - the safety gap,
    /// then a new token will be created when calling <see cref="M:SharedKernel.Infrastructure.PayPal.Api.OAuthTokenCredential.GetAccessToken" />.
    /// </summary>
    public int AccessTokenExpirationSafetyGapInSeconds { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    public OAuthTokenCredential(Dictionary<string, string> config)
        : this(string.Empty, string.Empty, config)
    {
    }

    /// <summary>Client Id and Secret for the OAuth</summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    public OAuthTokenCredential(string clientId, string clientSecret)
        : this(clientId, clientSecret, null)
    {
    }

    /// <summary>Client Id and Secret for the OAuth</summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="config"></param>
    public OAuthTokenCredential(
        string clientId = "",
        string clientSecret = "",
        Dictionary<string, string>? config = null)
    {
        _config = config != null ? ConfigManager.GetConfigWithDefaults(config) : ConfigManager.GetConfigWithDefaults(ConfigManager.Instance.GetProperties());
        if (string.IsNullOrEmpty(clientId))
        {
            ClientId = _config.TryGetValue(nameof(clientId), out var value) ? value : string.Empty;
        }
        else
        {
            ClientId = clientId;
            _config[nameof(clientId)] = clientId;
        }
        if (string.IsNullOrEmpty(clientSecret))
        {
            ClientSecret = _config.TryGetValue(nameof(clientSecret), out var value) ? value : string.Empty;
        }
        else
        {
            ClientSecret = clientSecret;
            _config[nameof(clientSecret)] = clientSecret;
        }
        _sdkVersion = new SdkVersion();
        AccessTokenExpirationSafetyGapInSeconds = 120;
    }

    /// <summary>
    /// Returns the currently cached access token. If no access token was
    /// previously cached, or if the current access token is expired, then
    /// a new one is generated and returned.
    /// </summary>
    /// <returns>The OAuth access token to use for making PayPal requests.</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.MissingCredentialException">Thrown if clientId or clientSecret are null or empty.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.InvalidCredentialException">Thrown if there is an issue converting the credentials to a formatted authorization string.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.IdentityException">Thrown if authorization fails as a result of providing invalid credentials.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.HttpException">Thrown if authorization fails and an HTTP error response is received.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.ConnectionException">Thrown if there is an issue attempting to connect to PayPal's services.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.ConfigException">Thrown if there is an error with any informaiton provided by the <see cref="T:SharedKernel.Infrastructure.PayPal.Api.ConfigManager" />.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown for any other general exception. See inner exception for further details.</exception>
    public string GetAccessToken()
    {
        if (!string.IsNullOrEmpty(_accessToken) && (DateTime.Now - AccessTokenLastCreationDate).TotalSeconds > AccessTokenExpirationInSeconds - AccessTokenExpirationSafetyGapInSeconds)
            _accessToken = null;
        if (string.IsNullOrEmpty(_accessToken))
            _accessToken = GenerateOAuthToken();
        return _accessToken;
    }

    /// <summary>
    /// Generates a new OAuth token useing the specified client credentials in the authorization request.
    /// </summary>
    /// <returns>The OAuth access token to use for making PayPal requests.</returns>
    private string GenerateOAuthToken()
    {
        var jobject = (JObject)JsonConvert.DeserializeObject(PayPalResource.ConfigureAndExecute<string>(new APIContext()
        {
            Config = _config,
            SdkVersion = _sdkVersion,
            HttpHeaders = new Dictionary<string, string>()
            {
                {
                    "Content-Type",
                    "application/x-www-form-urlencoded"
                },
            },
        }, PayPalResource.HttpMethod.Post, "/v1/oauth2/token", "grant_type=client_credentials", GetEndpointOverride()));
        var oauthToken = (string)jobject["token_type"] + " " + (string)jobject["access_token"];
        ApplicationId = (string)jobject["app_id"];
        AccessTokenExpirationInSeconds = (int)jobject["expires_in"];
        AccessTokenLastCreationDate = DateTime.Now;
        return oauthToken;
    }

    /// <summary>
    /// Gets the overridden endpoint defined in the config, if set.  Otherwise returns an empty string, in which case the default endpoint will be used when requesting a new token.
    /// </summary>
    /// <returns>An endpoint to use; empty string if no override is specified.</returns>
    private string GetEndpointOverride()
    {
        if (!_config.TryGetValue("oauth.EndPoint", out var endpointOverride))
            return string.Empty;

        if (!endpointOverride.EndsWith("/"))
            endpointOverride += "/";
        return endpointOverride;
    }
}