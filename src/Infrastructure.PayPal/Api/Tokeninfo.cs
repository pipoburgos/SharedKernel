using SharedKernel.Infrastructure.PayPal.Util;
using System.Text;
using System.Web;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Class that stores OpenIdConnect access token information.
/// </summary>
public class Tokeninfo : PayPalResource
{
    /// <summary>
    /// OPTIONAL, if identical to the scope requested by the client otherwise, REQUIRED
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Scope { get; set; }

    /// <summary>The access token issued by the authorization server</summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string AccessToken { get; set; }

    /// <summary>
    /// The refresh token, which can be used to obtain new access tokens using the same authorization grant as described in OAuth2.0 RFC6749 in Section 6
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string RefreshToken { get; set; }

    /// <summary>
    /// The type of the token issued as described in OAuth2.0 RFC6749 (Section 7.1), value is case insensitive
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TokenType { get; set; }

    /// <summary>The lifetime in seconds of the access token</summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int ExpiresIn { get; set; }

    /// <summary>Explicit default constructor</summary>
    public Tokeninfo()
    {
    }

    /// <summary>Constructor overload</summary>
    public Tokeninfo(string accessToken, string tokenType, int expiresIn)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
    }

    /// <summary>
    /// Creates an Access Token from an Authorization Code.
    /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
    /// </summary>
    public static Tokeninfo CreateFromAuthorizationCode(
        CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
    {
        return CreateFromAuthorizationCode(null, createFromAuthorizationCodeParameters);
    }

    /// <summary>
    /// Creates an Access Token from an Authorization Code.
    /// <param name="apiContext">APIContext to be used for the call.</param>
    /// <param name="createFromAuthorizationCodeParameters">Query parameters used for API call</param>
    /// </summary>
    public static Tokeninfo CreateFromAuthorizationCode(
        APIContext apiContext,
        CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
    {
        var resourcePath = SdkUtil.FormatUriPath("v1/identity/openidconnect/tokenservice?grant_type={0}&code={1}&redirect_uri={2}",
        [
            createFromAuthorizationCodeParameters,
        ]);
        return CreateFromAuthorizationCodeParameters(apiContext, createFromAuthorizationCodeParameters, resourcePath);
    }

    /// <summary>
    /// Creates Access and Refresh Tokens from an Authorization Code for future payments.
    /// </summary>
    /// <param name="apiContext">APIContext to be used for the call.</param>
    /// <param name="createFromAuthorizationCodeParameters">Query parameters used for the API call.</param>
    /// <returns>A TokenInfo object containing the Access and Refresh Tokens.</returns>
    public static Tokeninfo CreateFromAuthorizationCodeForFuturePayments(
        APIContext apiContext,
        CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters)
    {
        var resourcePath = SdkUtil.FormatUriPath("v1/oauth2/token?grant_type=authorization_code&response_type=token&redirect_uri=urn:ietf:wg:oauth:2.0:oob&code={0}",
        [
            createFromAuthorizationCodeParameters.ContainerMap["code"],
        ]);
        return CreateFromAuthorizationCodeParameters(apiContext, createFromAuthorizationCodeParameters, resourcePath);
    }

    /// <summary>
    /// Helper method for creating Access and Refresh Tokens from an Authorization Code.
    /// </summary>
    /// <param name="apiContext">APIContext to be used for the call.</param>
    /// <param name="createFromAuthorizationCodeParameters">Query parameters used for the API call.</param>
    /// <param name="resourcePath">The path to the REST API resource that will be requested.</param>
    /// <returns>A TokenInfo object containing the Access and Refresh Tokens.</returns>
    private static Tokeninfo CreateFromAuthorizationCodeParameters(
        APIContext apiContext,
        CreateFromAuthorizationCodeParameters createFromAuthorizationCodeParameters,
        string resourcePath)
    {
        var payload = resourcePath.Substring(resourcePath.IndexOf('?') + 1);
        resourcePath = resourcePath.Substring(0, resourcePath.IndexOf("?"));
        var dictionary = new Dictionary<string, string>();
        dictionary.Add("Content-Type", "application/x-www-form-urlencoded");
        if (apiContext == null)
            apiContext = new APIContext();
        apiContext.HttpHeaders = dictionary;
        apiContext.MaskRequestId = true;
        return ConfigureAndExecute<Tokeninfo>(apiContext, HttpMethod.Post, resourcePath, payload);
    }

    /// <summary>
    /// Creates an Access Token from an Refresh Token.
    /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
    /// </summary>
    public Tokeninfo CreateFromRefreshToken(
        CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
    {
        return CreateFromRefreshToken(null, createFromRefreshTokenParameters);
    }

    /// <summary>
    /// Creates an Access Token from an Refresh Token
    /// <param name="apiContext">APIContext to be used for the call</param>
    /// <param name="createFromRefreshTokenParameters">Query parameters used for API call</param>
    /// </summary>
    public Tokeninfo CreateFromRefreshToken(
        APIContext apiContext,
        CreateFromRefreshTokenParameters createFromRefreshTokenParameters)
    {
        if (!createFromRefreshTokenParameters.ContainerMap.ContainsKey("client_id"))
            createFromRefreshTokenParameters.ContainerMap["client_id"] = apiContext.Config["clientId"];
        if (!createFromRefreshTokenParameters.ContainerMap.ContainsKey("client_secret"))
            createFromRefreshTokenParameters.ContainerMap["client_secret"] = apiContext.Config["clientSecret"];
        var pattern = "v1/identity/openidconnect/tokenservice?grant_type={0}&refresh_token={1}&scope={2}&client_id={3}&client_secret={4}";
        createFromRefreshTokenParameters.SetRefreshToken(HttpUtility.UrlEncode(RefreshToken));
        var parameters = new object[1]
        {
            createFromRefreshTokenParameters,
        };
        var str = SdkUtil.FormatUriPath(pattern, parameters);
        var payload = str.Substring(str.IndexOf('?') + 1);
        var resource = str.Substring(0, str.IndexOf("?"));
        var dictionary = new Dictionary<string, string>();
        dictionary.Add("Content-Type", "application/x-www-form-urlencoded");
        if (apiContext == null)
            apiContext = new APIContext();
        apiContext.HttpHeaders = dictionary;
        apiContext.MaskRequestId = true;
        var bytes = Encoding.UTF8.GetBytes($"{apiContext.Config["clientId"]}:{apiContext.Config["clientSecret"]}");
        apiContext.AccessToken = "Basic " + Convert.ToBase64String(bytes);
        return ConfigureAndExecute<Tokeninfo>(apiContext, HttpMethod.Post, resource, payload);
    }
}