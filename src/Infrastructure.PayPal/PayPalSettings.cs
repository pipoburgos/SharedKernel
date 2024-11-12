using PayPal.Exceptions;

namespace SharedKernel.Infrastructure.PayPal;

/// <summary> . </summary>
public class PayPalSettings
{
    /// <summary> . </summary>
    public string Mode { get; set; } = "sandbox";

    /// <summary> . </summary>
    public Uri TokenEndpoint { get; set; } = new Uri("https://api-m.sandbox.paypal.com/v1/oauth2/token");

    /// <summary> . </summary>
    public string? ClientId { get; set; }

    /// <summary> . </summary>
    public string? ClientSecret { get; set; }

    /// <summary> . </summary>
    public int RequestRetries { get; set; } = 1;

    /// <summary> . </summary>
    public int ConnectionTimeout { get; set; } = 360000;

    /// <summary> . </summary>
    public string? ProxyAddress { get; set; }

    /// <summary> . </summary>
    public string? ProxyCredentials { get; set; }

    /// <summary>
    /// Gets the endpoint to be used when making an HTTP call to the REST API.
    /// </summary>
    /// <returns>The endpoint to be used when making an HTTP call to the REST API.</returns>
    public Uri GetEndpoint()
    {
        switch (Mode)
        {
            case "live":
                return new Uri("https://api.paypal.com/");
            case "sandbox":
                return new Uri("https://api.sandbox.paypal.com/");
            case "security-test-sandbox":
                return new Uri("https://test-api.sandbox.paypal.com/");
            default:
                throw new PayPalException("Mode not found. (live, sandbox, security-test-sandbox)");
        }
    }
}