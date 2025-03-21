﻿namespace PayPal.V1.Shared;

/// <summary>
/// Parameters for getting an access token using a authorization code.
/// </summary>
public class CreateFromAuthorizationCodeParameters : ClientCredentials
{
    /// <summary>Backing map</summary>
    public Dictionary<string, string> ContainerMap { get; set; }

    /// <summary>
    /// Initializes a new <seealso cref="T:PayPal.V1.Shared.CreateFromAuthorizationCodeParameters" /> object and sets the grant type to 'authorization_code'.
    /// </summary>
    public CreateFromAuthorizationCodeParameters()
    {
        ContainerMap = new Dictionary<string, string>();
        SetGrantType("authorization_code");
    }

    /// <summary>Set the code</summary>
    /// <param name="code"></param>
    public void SetCode(string code)
    {
        ContainerMap[nameof(code)] = code;
    }

    /// <summary>Set the Redirect URI</summary>
    /// <param name="redirectUri"></param>
    public void SetRedirectUri(string redirectUri)
    {
        ContainerMap["redirect_uri"] = redirectUri;
    }

    /// <summary>Set the Grant Type</summary>
    /// <param name="grantType"></param>
    public void SetGrantType(string grantType)
    {
        ContainerMap["grant_type"] = grantType;
    }
}