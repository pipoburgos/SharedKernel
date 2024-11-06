using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// One or more webhook objects.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Webhook : PayPalRelationalObject
{
    /// <summary>The ID of the webhook.</summary>
    public string Id { get; set; }

    /// <summary>
    /// The URL that is configured to listen on `localhost` for incoming `POST` notification messages that contain event information.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// A list of up to ten events to which to subscribe your webhook. To subscribe to all events including new events as they are added, specify the asterisk (`*`) wildcard. To replace the `event_types` array, specify the `*` wildcard. To see all supported events, [list available events](#available-event-type.list).
    /// </summary>
    public List<WebhookEventType> EventTypes { get; set; }

    /// <summary>
    /// Subscribes your webhook listener to events. A successful call returns a [`webhook`](/docs/api/webhooks/#definition-webhook) object, which includes the webhook ID for later use.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Webhook</returns>
    public Webhook Create(APIContext apiContext)
    {
        return Create(apiContext, this);
    }

    /// <summary>
    /// Creates the Webhook for the application associated with the access token.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhook"><seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.Webhook" /> object to be created.</param>
    /// <returns>Webhook</returns>
    public static Webhook Create(APIContext apiContext, Webhook webhook)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/notifications/webhooks";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, webhook);
    }

    /// <summary>Shows details for a webhook, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhookId">The ID of the webhook for which to show details.</param>
    /// <returns>Webhook</returns>
    public static Webhook Get(APIContext apiContext, string webhookId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(webhookId, nameof(webhookId));
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks/{0}", [
            webhookId,
        ]);
        return ConfigureAndExecute<Webhook>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>Lists all webhooks for an app.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="anchorType">Filters the response by an entity type, `anchor_id`. Value is `APPLICATION` or `ACCOUNT`. Default is `APPLICATION`.</param>
    /// <returns>WebhookList</returns>
    public static WebhookList GetAll(APIContext apiContext, string anchorType = null)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters();
        if (anchorType != null)
        {
            ArgumentValidator.Validate(anchorType, nameof(anchorType));
            queryParameters["anchor_type"] = anchorType;
        }
        var resource = "v1/notifications/webhooks" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<WebhookList>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Replaces webhook fields with new values. Pass a `json_patch` object with `replace` operation and `path`, which is `/url` for a URL or `/event_types` for events. The `value` is either the URL or a list of events.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>Webhook</returns>
    public Webhook Update(APIContext apiContext, PatchRequest patchRequest)
    {
        return Update(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Updates the Webhook identified by webhook_id for the application associated with access token.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhookId">ID of the webhook to be updated.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>Webhook</returns>
    public static Webhook Update(
        APIContext apiContext,
        string webhookId,
        PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(webhookId, nameof(webhookId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks/{0}", [
            webhookId,
        ]);
        return ConfigureAndExecute<Webhook>(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>Deletes a webhook, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Delete(APIContext apiContext)
    {
        Delete(apiContext, Id);
    }

    /// <summary>Deletes a webhook, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhookId">The ID of the webhook to delete.</param>
    public static void Delete(APIContext apiContext, string webhookId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(webhookId, nameof(webhookId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks/{0}", [
            webhookId,
        ]);
        ConfigureAndExecute<Webhook>(apiContext, HttpMethod.Delete, resource);
    }
}