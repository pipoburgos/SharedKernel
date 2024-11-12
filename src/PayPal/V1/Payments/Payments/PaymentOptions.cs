
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Payment options requested for this purchase unit
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentOptions //: PayPalSerializableObject
{
    /// <summary>Payment method requested for this purchase unit</summary>
    public string? AllowedPaymentMethod { get; set; }
}