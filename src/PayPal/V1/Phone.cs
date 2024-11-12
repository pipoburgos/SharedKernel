
namespace PayPal.V1;

/// <summary>
/// Information related to the Payee.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Phone //: PayPalSerializableObject
{
    /// <summary>Country code (from in E.164 format)</summary>
    public string? CountryCode { get; set; }

    /// <summary>In-country phone number (from in E.164 format)</summary>
    public string? NationalNumber { get; set; }

    /// <summary>Phone extension</summary>
    public string? Extension { get; set; }
}