using SharedKernel.Infrastructure.PayPal.Exceptions;

namespace SharedKernel.Infrastructure.PayPal;

public class PayPalSettings
{
    public string Mode { get; set; } = "sandbox";

    public Uri TokenEndpoint { get; set; } = new Uri("https://api-m.sandbox.paypal.com/v1/oauth2/token");

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public int RequestRetries { get; set; } = 1;
    public int ConnectionTimeout { get; set; } = 360000;

    public string? ProxyAddress { get; set; }

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