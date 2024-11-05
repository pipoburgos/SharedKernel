using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A list of events.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebhookEventType : PayPalResource
{
    /// <summary>The unique event name.</summary>
    public string Name { get; set; }

    /// <summary>A human-readable description of the event.</summary>
    public string Description { get; set; }

    /// <summary>The status of a webhook event.</summary>
    public string Status { get; set; }

    /// <summary>Lists event subscriptions for a webhook, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhookId">The ID of the webhook for which to list subscriptions.</param>
    /// <returns>WebhookEventTypeList</returns>
    public static WebhookEventTypeList SubscribedEventTypes(APIContext apiContext, string webhookId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(webhookId, nameof (webhookId));
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks/{0}/event-types", [
            webhookId,
        ]);
        return ConfigureAndExecute<WebhookEventTypeList>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Lists available events to which any webhook can subscribe. For a list of supported events, see [Webhook events](/docs/integration/direct/rest/webhooks/webhook-events/).
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>WebhookEventTypeList</returns>
    public static WebhookEventTypeList AvailableEventTypes(APIContext apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var resource = "v1/notifications/webhooks-event-types";
        return ConfigureAndExecute<WebhookEventTypeList>(apiContext, HttpMethod.Get, resource);
    }
}