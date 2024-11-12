
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Object used to store the currency conversion rate.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CurrencyConversion : PayPalRelationalObject
{
    /// <summary>Date of validity for the conversion rate.</summary>
    public string? ConversionDate { get; set; }

    /// <summary>3 letter currency code</summary>
    public string? FromCurrency { get; set; }

    /// <summary>
    /// Amount participating in currency conversion, set to 1 as default
    /// </summary>
    public string? FromAmount { get; set; }

    /// <summary>3 letter currency code</summary>
    public string? ToCurrency { get; set; }

    /// <summary>Amount resulting from currency conversion.</summary>
    public string? ToAmount { get; set; }

    /// <summary>Field indicating conversion type applied.</summary>
    public string? ConversionType { get; set; }

    /// <summary>Allow Payer to change conversion type.</summary>
    public bool? ConversionTypeChangeable { get; set; }

    /// <summary>Base URL to web applications endpoint</summary>
    public string? WebUrl { get; set; }
}