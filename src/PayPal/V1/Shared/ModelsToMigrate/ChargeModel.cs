
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Defines a charge model to be used in context of a billing plan.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ChargeModel //: PayPalSerializableObject
{
    /// <summary>Identifier of the charge model. 128 characters max.</summary>
    public string? Id { get; set; }

    /// <summary>
    /// Type of charge model. Allowed values: `SHIPPING`, `TAX`.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>Specific amount for this charge model.</summary>
    public PayPalCurrency? Amount { get; set; }
}