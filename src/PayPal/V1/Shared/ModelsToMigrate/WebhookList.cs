
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// List of webhooks.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebhookList //: PayPalSerializableObject
{
    /// <summary>A list of webhooks.</summary>
    public List<Webhook> Webhooks { get; set; }
}