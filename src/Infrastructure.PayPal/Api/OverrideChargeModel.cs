
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Override for a charge model when creating a billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class OverrideChargeModel //: PayPalSerializableObject
{
    /// <summary>ID of charge model.</summary>
    public string ChargeId { get; set; }

    /// <summary>
    /// Updated Amount to be associated with this charge model.
    /// </summary>
    public PayPalCurrency Amount { get; set; }
}