
namespace PayPal.V1;

/// <summary>
/// Base object for all financial value related fields (balance, payment due, etc.)
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayPalCurrency //: PayPalSerializableObject
{
    /// <summary>3 letter currency code</summary>
    public string? Currency { get; set; }

    /// <summary>amount upto 2 decimals represented as string</summary>
    public string? Value { get; set; }
}