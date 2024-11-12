
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// List of webhook events.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebhookEventTypeList //: PayPalSerializableObject
{
    /// <summary>A list of webhook events.</summary>
    public List<WebhookEventType> EventTypes { get; set; }
}