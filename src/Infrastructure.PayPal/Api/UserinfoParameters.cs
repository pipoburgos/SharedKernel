using System.Web;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Class for storing user information for OpenIdConnect API calls.
/// </summary>
public class UserinfoParameters
{
    /// <summary>Schema used in query parameters</summary>
    private const string Schema = "schema";
    /// <summary>Access Token used in query parameters</summary>
    private const string AccessToken = "access_token";

    /// <summary>Constructor</summary>
    public UserinfoParameters()
    {
        ContainerMap = new Dictionary<string, string>();
        ContainerMap.Add("schema", "openid");
    }

    /// <summary>Gets and sets the backing map</summary>
    public Dictionary<string, string> ContainerMap { get; set; }

    /// <summary>Set the Access Token</summary>
    /// <param name="accessToken"></param>
    public void SetAccessToken(string accessToken)
    {
        ContainerMap["access_token"] = HttpUtility.UrlEncode(accessToken);
    }
}