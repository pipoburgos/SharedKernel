
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The payment and refund summary.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentSummary //: PayPalSerializableObject
{
    /// <summary>The total amount paid or refunded through PayPal.</summary>
    public PayPalCurrency Paypal { get; set; }

    /// <summary>
    /// The total amount paid or refunded through other sources.
    /// </summary>
    public PayPalCurrency Other { get; set; }
}