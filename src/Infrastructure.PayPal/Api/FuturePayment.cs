using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A REST API future payment resource.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FuturePayment : Payment
{
    /// <summary>
    /// Creates a future payment using the specified API context and correlation ID.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="correlationId">(Optional) Application correlation ID</param>
    /// <returns>A new payment object setup to be used for a future payment.</returns>
    public Payment Create(APIContext apiContext, string correlationId = "")
    {
        return Create(apiContext, this, correlationId);
    }

    /// <summary>
    /// Creates a future payment using the specified API context and correlation ID.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="payment">FuturePayment object to be used in creating the PayPal resource.</param>
    /// <param name="correlationId">(Optional) Application correlation ID</param>
    /// <returns>A new payment object setup to be used for a future payment.</returns>
    public static Payment Create(
        APIContext apiContext,
        FuturePayment payment,
        string correlationId = "")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        if (!string.IsNullOrEmpty(correlationId))
            apiContext.HttpHeaders["PAYPAL-CLIENT-METADATA-ID"] = correlationId;
        return Payment.Create(apiContext, payment);
    }
}