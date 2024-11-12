
using PayPal.V1.Shared;

namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Payee information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class DisplayPhone : PayPalRelationalObject
{
    /// <summary>
    /// The country code in [E.164 format](https://en.wikipedia.org/wiki/E.164).
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// The in-country phone number in [E.164 format](https://en.wikipedia.org/wiki/E.164).
    /// </summary>
    public string? Number { get; set; }
}