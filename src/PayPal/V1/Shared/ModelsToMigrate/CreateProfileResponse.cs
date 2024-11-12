
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Response object when creating a web experience profile.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CreateProfileResponse //: PayPalSerializableObject
{
    /// <summary>ID of the payment web experience profile.</summary>
    public string? Id { get; set; }
}