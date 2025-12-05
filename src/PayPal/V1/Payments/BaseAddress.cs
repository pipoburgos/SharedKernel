namespace PayPal.V1.Payments;

/// <summary>
/// Base Address object used as billing address in a payment or extended for Shipping Address.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class BaseAddress
{
    /// <summary>Line 1 of the Address (eg. number, street, etc).</summary>
    public string? Line1 { get; set; }

    /// <summary>
    /// Optional line 2 of the Address (eg. suite, apt #, etc.).
    /// </summary>
    public string? Line2 { get; set; }

    /// <summary>City name.</summary>
    public string? City { get; set; }

    /// <summary>2 letter country code.</summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Zip code or equivalent is usually required for countries that have them. For list of countries that do not have postal codes please refer to http://en.wikipedia.org/wiki/Postal_code.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// 2 letter code for US states, and the equivalent for other countries.
    /// </summary>
    public string? State { get; set; }
}