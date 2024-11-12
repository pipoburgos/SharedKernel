
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Let's you execute a PayPal Account based Payment resource with the payer_id obtained from web approval url.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentExecution //: PayPalSerializableObject
{
    /// <summary>
    /// The ID of the Payer, passed in the `return_url` by PayPal.
    /// </summary>
    public string? PayerId { get; set; }

    /// <summary>
    /// Transactional details including the amount and item details.
    /// </summary>
    public List<Transaction>? Transactions { get; set; }
}