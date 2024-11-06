using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// An authorization transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Authorization : PayPalRelationalObject
{
    /// <summary>ID of the authorization transaction.</summary>
    public string Id { get; set; }

    /// <summary>Amount being authorized.</summary>
    public Amount Amount { get; set; }

    /// <summary>Specifies the payment mode of the transaction.</summary>
    public string PaymentMode { get; set; }

    /// <summary>State of the authorization.</summary>
    public string State { get; set; }

    /// <summary>
    /// Reason code, `AUTHORIZATION`, for a transaction state of `pending`.
    /// </summary>
    public string ReasonCode { get; set; }

    public string ProtectionEligibility { get; set; }

    public string ProtectionEligibilityType { get; set; }

    /// <summary>
    /// Fraud Management Filter (FMF) details applied for the payment that could result in accept, deny, or pending action. Returned in a payment response only if the merchant has enabled FMF in the profile settings and one of the fraud filters was triggered based on those settings. See [Fraud Management Filters Summary](https://developer.paypal.com/docs/classic/fmf/integration-guide/FMFSummary/) for more information.
    /// </summary>
    public FmfDetails FmfDetails { get; set; }

    /// <summary>
    /// ID of the Payment resource that this transaction is based on.
    /// </summary>
    public string ParentPayment { get; set; }

    /// <summary>
    /// Authorization expiration time and date as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string ValidUntil { get; set; }

    /// <summary>
    /// Time of authorization as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>Time that the resource was last updated.</summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// Collection of payment response related fields returned from a payment request.
    /// </summary>
    public ProcessorResponse ProcessorResponse { get; set; }

    /// <summary>
    /// Identifier to the purchase or transaction unit corresponding to this authorization transaction.
    /// </summary>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Receipt id is 16 digit number payment identification number returned for guest users to identify the payment.
    /// </summary>
    public string ReceiptId { get; set; }

    /// <summary>Shows details for an authorization, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="authorizationId">The ID of the authorization for which to show details.</param>
    /// <returns>Authorization</returns>
    public static Authorization Get(APIContext apiContext, string authorizationId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(authorizationId, nameof(authorizationId));
        var resource = SdkUtil.FormatUriPath("v1/payments/authorization/{0}", [
            authorizationId,
        ]);
        return ConfigureAndExecute<Authorization>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Captures and processes an authorization, by ID. To use this call, the original payment call must specify an intent of `authorize`.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="capture">Capture</param>
    /// <returns>Capture</returns>
    public Capture Capture(APIContext apiContext, Capture capture)
    {
        return Capture(apiContext, Id, capture);
    }

    /// <summary>
    /// Creates (and processes) a new Capture Transaction added as a related resource.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="authorizationId">ID of the authorization to capture.</param>
    /// <param name="capture">Capture</param>
    /// <returns>Capture</returns>
    public static Capture Capture(
        APIContext apiContext,
        string authorizationId,
        Capture capture)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(authorizationId, nameof(authorizationId));
        ArgumentValidator.Validate(capture, nameof(capture));
        var resource = SdkUtil.FormatUriPath("v1/payments/authorization/{0}/capture", [
            authorizationId,
        ]);
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, capture);
    }

    /// <summary>
    /// Voids, or cancels, an authorization, by ID. You cannot void a fully captured authorization.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Authorization</returns>
    public Authorization Void(APIContext apiContext)
    {
        return Void(apiContext, Id);
    }

    /// <summary>Voids (cancels) an Authorization.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="authorizationId">ID of the authorization to void.</param>
    /// <returns>Authorization</returns>
    public static Authorization Void(APIContext apiContext, string authorizationId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(authorizationId, nameof(authorizationId));
        var resource = SdkUtil.FormatUriPath("v1/payments/authorization/{0}/void", [
            authorizationId,
        ]);
        return ConfigureAndExecute<Authorization>(apiContext, HttpMethod.Post, resource);
    }

    /// <summary>
    /// Reauthorizes a PayPal account payment, by authorization ID. To ensure that funds are still available, reauthorize a payment after the initial three-day honor period. Supports only the `amount` request parameter.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Authorization</returns>
    public Authorization Reauthorize(APIContext apiContext)
    {
        return Reauthorize(apiContext, this);
    }

    /// <summary>Reauthorizes an expired Authorization.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="authorization">The Authorization object containing the details of the authorization that should be reauthorized.</param>
    /// <returns>Authorization</returns>
    public static Authorization Reauthorize(APIContext apiContext, Authorization authorization)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(authorization, nameof(authorization));
        var resource = SdkUtil.FormatUriPath("v1/payments/authorization/{0}/reauthorize", [
            authorization.Id,
        ]);
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, authorization);
    }
}