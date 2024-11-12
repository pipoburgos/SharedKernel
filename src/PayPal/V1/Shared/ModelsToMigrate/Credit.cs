
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// A resource representing a credit instrument.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Credit //: PayPalSerializableObject
{
    /// <summary>Unique identifier of credit resource.</summary>
    public string? Id { get; set; }

    /// <summary>specifies type of credit</summary>
    public string? Type { get; set; }
}