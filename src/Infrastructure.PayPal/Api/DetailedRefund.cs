
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A refund transaction. This is the resource that is returned on GET /refund
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class DetailedRefund : Refund
{
    /// <summary>free-form field for the use of clients</summary>
    public string? Custom { get; set; }

    /// <summary>invoice number to track this payment</summary>
    public new string? InvoiceNumber { get; set; }

    /// <summary>
    /// Amount refunded to payer of the original transaction, in the current Refund call
    /// </summary>
    public PayPalCurrency? RefundToPayer { get; set; }

    /// <summary>
    /// List of external funding that were refunded by the Refund call. Each external_funding unit should have a unique reference_id
    /// </summary>
    public List<ExternalFunding>? RefundToExternalFunding { get; set; }

    /// <summary>
    /// Transaction fee refunded to original recipient of payment.
    /// </summary>
    public PayPalCurrency? RefundFromTransactionFee { get; set; }

    /// <summary>
    /// Amount subtracted from PayPal balance of the original recipient of payment, to make this refund.
    /// </summary>
    public PayPalCurrency? RefundFromReceivedAmount { get; set; }

    /// <summary>
    /// Total amount refunded so far from the original purchase. Say, for example, a buyer makes $100 purchase, the buyer was refunded $20 a week ago and is refunded $30 in this transaction. The gross refund amount is $30 (in this transaction). The total refunded amount is $50.
    /// </summary>
    public PayPalCurrency? TotalRefundedAmount { get; set; }
}