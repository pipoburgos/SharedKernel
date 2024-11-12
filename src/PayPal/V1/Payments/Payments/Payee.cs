
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// A resource representing a Payee who receives the funds and fulfills the order.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Payee //: PayPalSerializableObject
{
    /// <summary>
    /// Email Address associated with the Payee's PayPal Account. If the provided email address is not associated with any PayPal Account, the payee can only receive PayPal Wallet Payments. Direct Credit Card Payments will be denied due to card compliance requirements.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>Encrypted PayPal account identifier for the Payee.</summary>
    public string? MerchantId { get; set; }

    /// <summary>
    /// Information related to the Payer. In case of PayPal Wallet payment, this information will be filled in by PayPal after the user approves the payment using their PayPal Wallet.
    /// </summary>
    public Phone? Phone { get; set; }

    /// <summary>Displays only metadata for a payee.</summary>
    public PayeeDisplayMetadata? PayeeDisplayMetadata { get; set; }
}