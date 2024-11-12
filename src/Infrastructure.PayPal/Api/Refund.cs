using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A refund transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Refund : PayPalRelationalObject
{
    /// <summary>ID of the refund transaction. 17 characters max.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Details including both refunded amount (to payer) and refunded fee (to payee). 10 characters max.
    /// </summary>
    public Amount Amount { get; set; }

    /// <summary>State of the refund.</summary>
    public string State { get; set; }

    /// <summary>
    /// Reason description for the Sale transaction being refunded.
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Your own invoice or tracking ID number. Character length and limitations: 127 single-byte alphanumeric characters.
    /// </summary>
    public string InvoiceNumber { get; set; }

    /// <summary>ID of the Sale transaction being refunded.</summary>
    public string SaleId { get; set; }

    /// <summary>ID of the sale transaction being refunded.</summary>
    public string CaptureId { get; set; }

    /// <summary>
    /// ID of the payment resource on which this transaction is based.
    /// </summary>
    public string ParentPayment { get; set; }

    /// <summary>Description of what is being refunded for.</summary>
    public string Description { get; set; }

    /// <summary>
    /// Time of refund as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>Time that the resource was last updated.</summary>
    public string UpdateTime { get; set; }

    /// <summary>The reason code for the refund state being pending</summary>
    public string ReasonCode { get; set; }

    /// <summary>Shows details for a refund, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="refundId">The ID of the refund for which to show details.</param>
    /// <returns>Refund</returns>
    public static Refund Get(IPayPalClient apiContext, string refundId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(refundId, nameof(refundId));
        var resource = SdkUtil.FormatUriPath("v1/payments/refund/{0}", [
            refundId,
        ]);
        return ConfigureAndExecute<Refund>(apiContext, HttpMethod.Get, resource);
    }
}