
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// List of webhooks events.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebhookEventList : PayPalRelationalObject
{
    /// <summary>A list of webhooks events.</summary>
    public List<WebhookEvent> Events { get; set; }

    /// <summary>
    /// The number of items in each range of results. Note that the response might have fewer items than the requested `page_size` value.
    /// </summary>
    public int Count { get; set; }
}