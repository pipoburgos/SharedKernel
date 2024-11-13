// ReSharper disable UnusedParameter.Global

namespace PayPal.V1.Shared;

/// <summary>
/// Abstract class that handles configuring an HTTP request prior to making an API call.
/// </summary>
public abstract class PayPalResource ////: PayPalSerializableObject
{
    /// <summary>PayPal debug id from response header</summary>
    public string? DebugId { get; set; }

    /// <summary>
    /// Gets the last request sent by the SDK in the current thread.
    /// </summary>
    public static ThreadLocal<RequestDetails> LastRequestDetails { get; private set; }

    /// <summary>
    /// Gets the last response received by the SDK in the current thread.
    /// </summary>
    public static ThreadLocal<ResponseDetails> LastResponseDetails { get; private set; }

    /// <summary>
    /// Static constructor initializing any static properties.
    /// </summary>
    static PayPalResource()
    {
        LastRequestDetails = new ThreadLocal<RequestDetails>();
        LastResponseDetails = new ThreadLocal<ResponseDetails>();
    }

    /// <summary>Configures and executes REST call: Supports JSON</summary>
    /// <param name="apiContext">IPayPalClient object</param>
    /// <param name="httpMethod">HttpMethod type</param>
    /// <param name="relativeUri">URI path of the resource</param>
    /// <param name="payload">JSON request payload</param>
    /// <returns>Response object or null otherwise for void API calls</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.HttpException">Thrown if there was an error sending the request.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PaymentsException">Thrown if an HttpException was raised and contains a Payments API error object.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown for any other issues encountered. See inner exception for further details.</exception>
    internal static T ConfigureAndExecute<T>(IPayPalClient apiContext, string httpMethod, string relativeUri, T payload)
    {
        return ConfigureAndExecute<T>(apiContext, httpMethod, relativeUri, (object?)payload);
    }

    /// <summary>Configures and executes REST call: Supports JSON</summary>
    /// <param name="apiContext">IPayPalClient object</param>
    /// <param name="httpMethod">HttpMethod type</param>
    /// <param name="relativeUri">URI path of the resource</param>
    /// <param name="payload">JSON request payload</param>
    /// <returns>Response object or null otherwise for void API calls</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.HttpException">Thrown if there was an error sending the request.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PaymentsException">Thrown if an HttpException was raised and contains a Payments API error object.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown for any other issues encountered. See inner exception for further details.</exception>
    internal static T ConfigureAndExecute<T>(IPayPalClient apiContext, string httpMethod, string relativeUri,
        PatchRequest payload)
    {
        return ConfigureAndExecute<T>(apiContext, httpMethod, relativeUri, (object?)payload);
    }

    /// <summary>Configures and executes REST call: Supports JSON</summary>
    /// <typeparam name="T">Generic Type parameter for response object</typeparam>
    /// <param name="apiContext">IPayPalClient object</param>
    /// <param name="httpMethod">HttpMethod type</param>
    /// <param name="relativeUri">URI path of the resource</param>
    /// <param name="payload">JSON request payload</param>
    /// <returns>Response object or null otherwise for void API calls</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.HttpException">Thrown if there was an error sending the request.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PaymentsException">Thrown if an HttpException was raised and contains a Payments API error object.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown for any other issues encountered. See inner exception for further details.</exception>
    internal static T ConfigureAndExecute<T>(IPayPalClient apiContext, string httpMethod, string relativeUri,
        object? payload = null)
    {
        return apiContext.Send<T>(httpMethod, relativeUri, payload);
    }
}