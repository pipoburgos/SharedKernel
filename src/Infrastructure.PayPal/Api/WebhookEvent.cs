using SharedKernel.Infrastructure.PayPal.Exceptions;
using SharedKernel.Infrastructure.PayPal.Util;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A webhook event notification.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebhookEvent : PayPalRelationalObject
{
    /// <summary>The ID of the webhook event notification.</summary>
    public string Id { get; set; }

    /// <summary>
    /// The date and time when the webhook event notification was created.
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// The name of the resource related to the webhook notification event.
    /// </summary>
    public string ResourceType { get; set; }

    /// <summary>The version of the event.</summary>
    public string EventVersion { get; set; }

    /// <summary>
    /// The event that triggered the webhook event notification.
    /// </summary>
    public string EventType { get; set; }

    /// <summary>
    /// A summary description for the event notification. For example, `A payment authorization was created.`
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// The resource that triggered the webhook event notification.
    /// </summary>
    public object Resource { get; set; }

    /// <summary>
    /// The event transmission status. Displayed only for internal API calls through the PayPal Developer portal.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// The list of transmissions for an event. Displayed only for internal API calls through the PayPal Developer portal.
    /// </summary>
    public List<object> Transmissions { get; set; }

    /// <summary>
    /// Shows details for a webhook event notification, by ID.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="eventId">The ID of the webhook event notification for which to show details.</param>
    /// <returns>WebhookEvent</returns>
    public static WebhookEvent Get(APIContext apiContext, string eventId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(eventId, nameof(eventId));
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks-events/{0}", [
            eventId,
        ]);
        return ConfigureAndExecute<WebhookEvent>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Resends the Webhooks event resource identified by event_id.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>WebhookEvent</returns>
    public WebhookEvent Resend(APIContext apiContext) => Resend(apiContext, Id);

    /// <summary>
    /// Resends the Webhooks event resource identified by event_id.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webhookEventId">ID of the webhook event to resend.</param>
    /// <returns>WebhookEvent</returns>
    public static WebhookEvent Resend(APIContext apiContext, string webhookEventId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(webhookEventId, nameof(webhookEventId));
        var resource = SdkUtil.FormatUriPath("v1/notifications/webhooks-events/{0}/resend", [
            webhookEventId,
        ]);
        return ConfigureAndExecute<WebhookEvent>(apiContext, HttpMethod.Post, resource);
    }

    /// <summary>
    /// Retrieves the list of Webhooks events resources for the application associated with token. The developers can use it to see list of past webhooks events.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="pageSize">Number of items to be returned by a GET operation</param>
    /// <param name="startTime">Resource creation time that indicates the start of a range of results.</param>
    /// <param name="endTime">Resource creation time that indicates the end of a range of results.</param>
    /// <returns>WebhookEventList</returns>
    public static WebhookEventList List(
        APIContext apiContext,
        int pageSize = 10,
        string startTime = "",
        string endTime = "")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            ["page_size"] = pageSize.ToString(),
            ["start_time"] = startTime,
            ["end_time"] = endTime,
        };
        var resource = "v1/notifications/webhooks-events" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<WebhookEventList>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Validates a received webhook event by checking the signature of the event and verifying the event originated from PayPal.
    /// </summary>
    /// <param name="apiContext">APIContext containing any configuration settings to be used when validating the event.</param>
    /// <param name="requestHeaders">A collection of HTTP request headers included with the received webhook event.</param>
    /// <param name="requestBody">The body of the received HTTP request.</param>
    /// <param name="webhookId">ID of the webhook resource associated with this webhook event. If not specified, it is assumed the ID is provided via the Config property of the <paramref name="apiContext" /> parameter.</param>
    /// <returns>True if the webhook event is valid and was sent from PayPal; false otherwise.</returns>
    public static bool ValidateReceivedEvent(
        APIContext apiContext,
        NameValueCollection requestHeaders,
        string requestBody,
        string webhookId = "")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        if (string.IsNullOrEmpty(webhookId))
            webhookId = apiContext.Config != null && apiContext.Config.ContainsKey("webhook.id") && !string.IsNullOrEmpty(apiContext.Config["webhook.id"]) ? apiContext.Config["webhook.id"] : throw new PayPalException("Webhook ID needed for event validation was not found. Ensure the 'webhook.id' key is included in your application's config file or provide the webhook ID when you call this method.");
        var requestHeader1 = requestHeaders["PAYPAL-TRANSMISSION-ID"];
        var requestHeader2 = requestHeaders["PAYPAL-TRANSMISSION-TIME"];
        var requestHeader3 = requestHeaders["PAYPAL-TRANSMISSION-SIG"];
        var requestHeader4 = requestHeaders["PAYPAL-CERT-URL"];
        var requestHeader5 = requestHeaders["PAYPAL-AUTH-ALGO"];
        ArgumentValidator.Validate(requestHeader1, "PAYPAL-TRANSMISSION-ID");
        ArgumentValidator.Validate(requestHeader2, "PAYPAL-TRANSMISSION-TIME");
        ArgumentValidator.Validate(requestHeader3, "PAYPAL-TRANSMISSION-SIG");
        ArgumentValidator.Validate(requestHeader4, "PAYPAL-CERT-URL");
        ArgumentValidator.Validate(requestHeader5, "PAYPAL-AUTH-ALGO");
        bool flag;
        try
        {
            var hashAlgorithmName = ConvertAuthAlgorithmHeaderToHashAlgorithmName(requestHeader5);
            var checksum = Crc32.ComputeChecksum(requestBody);
            var bytes = Encoding.UTF8.GetBytes($"{requestHeader1}|{requestHeader2}|{webhookId}|{checksum}");
            var certificatesFromUrl = CertificateManager.Instance.GetCertificatesFromUrl(requestHeader4);
            flag = CertificateManager.Instance.ValidateCertificateChain(CertificateManager.Instance.GetTrustedCertificateFromFile(apiContext == null ? null : apiContext.Config), certificatesFromUrl);
            if (flag)
            {
                var key = certificatesFromUrl[0].PublicKey.Key as RSACryptoServiceProvider;
                var numArray = Convert.FromBase64String(requestHeader3);
                var buffer = bytes;
                var oid = CryptoConfig.MapNameToOID(hashAlgorithmName);
                var signature = numArray;
                flag = key.VerifyData(buffer, oid, signature);
            }
        }
        catch (PayPalException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PayPalException("Encountered an error while attepting to validate a webhook event.", ex);
        }
        return flag;
    }

    /// <summary>
    /// Converts the algorithm name specified by <paramref name="authAlgorithmHeader" /> into a hash algorithm name recognized by <seealso cref="T:System.Security.Cryptography.CryptoConfig" />.
    /// </summary>
    /// <param name="authAlgorithmHeader">The PAYPAL-AUTH-ALGO header value included with a received Webhook event.</param>
    /// <returns>A mapped hash algorithm name.</returns>
    internal static string ConvertAuthAlgorithmHeaderToHashAlgorithmName(string authAlgorithmHeader)
    {
        var str = "withRSA";
        return authAlgorithmHeader.EndsWith(str) ? authAlgorithmHeader.Split([
            str,
        ], StringSplitOptions.None)[0] : throw new AlgorithmNotSupportedException(
            $"Unable to map {authAlgorithmHeader} to a known hash algorithm.");
    }
}