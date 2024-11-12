
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// A resource representing a incentive.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Incentive //: PayPalSerializableObject
{
    /// <summary>Identifier of the instrument in PayPal Wallet</summary>
    public string Id { get; set; }

    /// <summary>Code that identifies the incentive.</summary>
    public string Code { get; set; }

    /// <summary>Name of the incentive.</summary>
    public string Name { get; set; }

    /// <summary>Description of the incentive.</summary>
    public string Description { get; set; }

    /// <summary>
    /// Indicates incentive is applicable for this minimum purchase amount.
    /// </summary>
    public PayPalCurrency MinimumPurchaseAmount { get; set; }

    /// <summary>Logo image url for the incentive.</summary>
    public string LogoImageUrl { get; set; }

    /// <summary>expiry date of the incentive.</summary>
    public string ExpiryDate { get; set; }

    /// <summary>Specifies type of incentive</summary>
    public string Type { get; set; }

    /// <summary>URI to the associated terms</summary>
    public string Terms { get; set; }
}