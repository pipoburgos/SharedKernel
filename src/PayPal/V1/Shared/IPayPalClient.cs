namespace PayPal.V1.Shared;

/// <summary> . </summary>
public interface IPayPalClient
{
    /// <summary> . </summary>
    T Send<T>(string httpMethod, string relativeUri, object? body = null);
}
