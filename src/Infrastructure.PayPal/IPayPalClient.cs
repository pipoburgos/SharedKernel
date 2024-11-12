using SharedKernel.Infrastructure.PayPal.Api;

namespace SharedKernel.Infrastructure.PayPal;

/// <summary> . </summary>
public interface IPayPalClient
{
    /// <summary> . </summary>
    bool MaskRequestId { set; }

    /// <summary> . </summary>
    PayPalTokenResponse? Token { get; }

    /// <summary> . </summary>
    void AddHeader(string key, string value);

    /// <summary> . </summary>
    Task<T> Send<T>(HttpMethod httpMethod, string resource, object? payload = null, string? endpoint = null,
        bool setAuthorizationHeader = true, CancellationToken cancellationToken = default);
}
