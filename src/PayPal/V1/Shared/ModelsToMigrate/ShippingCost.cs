
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Shipping cost, as a percent or an amount value.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ShippingCost //: PayPalSerializableObject
{
    /// <summary>
    /// The shipping cost, as an amount value. Valid value is from 0 to 999999.99.
    /// </summary>
    public PayPalCurrency Amount { get; set; }

    /// <summary>The tax percentage on the shipping amount.</summary>
    public Tax Tax { get; set; }
}