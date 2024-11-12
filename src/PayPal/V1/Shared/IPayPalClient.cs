namespace PayPal.V1.Shared;

/// <summary> . </summary>
public interface IPayPalClient
{
    /// <summary> . </summary>
    T Send<T>(string httpMethod, string resource, object? payload = null, string? endpoint = null,
        bool setAuthorizationHeader = true, bool maskRequestId = false);
}
