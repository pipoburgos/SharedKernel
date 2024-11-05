namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>REST application client credentials.</summary>
public abstract class ClientCredentials
{
    /// <summary>Client ID</summary>
    public string ClientId { get; set; }

    /// <summary>Client Secret</summary>
    public string ClientSecret { get; set; }
}