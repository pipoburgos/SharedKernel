
namespace PayPal.V1.Payments;

/// <summary>
///  A resource representing installment information available for a transaction
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InstallmentInfo //: PayPalSerializableObject
{
    /// <summary>Installment id.</summary>
    public string? InstallmentId { get; set; }

    /// <summary>Credit card network.</summary>
    public string? Network { get; set; }

    /// <summary>Credit card issuer.</summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// List of available installment options and the cost associated with each one.
    /// </summary>
    public List<InstallmentOption>? InstallmentOptions { get; set; }
}