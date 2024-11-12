
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// A transaction defines the contract of a payment - what is the payment for and who is fulfilling it.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Transaction : CartBase
{
    /// <summary>
    /// Identifier to the purchase unit corresponding to this sale transaction.
    /// </summary>
    public string? PurchaseUnitReferenceId { get; set; }

    /// <summary>
    /// List of financial transactions (Sale, Authorization, Capture, Refund) related to the payment.
    /// </summary>
    public List<RelatedResources>? RelatedResources { get; set; }
}