namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Base Address object used as billing address in a payment or extended for Shipping Address.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Address : BaseAddress
{
    /// <summary>Phone number in E.123 format. 50 characters max.</summary>
    public string Phone { get; set; }

    /// <summary>
    /// Address normalization status, returned only for payers from Brazil.
    /// </summary>
    public new string NormalizationStatus { get; set; }

    /// <summary>Address status</summary>
    public new string Status { get; set; }

    /// <summary>Type of address (e.g., HOME_OR_WORK, GIFT etc).</summary>
    public string Type { get; set; }
}