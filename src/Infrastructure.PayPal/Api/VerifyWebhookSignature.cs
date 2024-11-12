using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Verify webhook signature.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class VerifyWebhookSignature : PayPalRelationalObject
{
    /// <summary>
    /// The algorithm that PayPal uses to generate the signature and that you can use to verify the signature. Extract this value from the `PAYPAL-AUTH-ALGO` response header, which is received with the webhook notification.
    /// </summary>
    public string AuthAlgo { get; set; }

    /// <summary>
    /// The X.509 public key certificate. Download the certificate from this URL and use it to verify the signature. Extract this value from the `PAYPAL-CERT-URL` response header, which is received with the webhook notification.
    /// </summary>
    public string CertUrl { get; set; }

    /// <summary>
    /// The ID of the HTTP transmission. Contained in the `PAYPAL-TRANSMISSION-ID` header of the notification message.
    /// </summary>
    public string TransmissionId { get; set; }

    /// <summary>
    /// The PayPal-generated asymmetric signature. Extract this value from the `PAYPAL-TRANSMISSION-SIG` response header, which is received with the webhook notification.
    /// </summary>
    public string TransmissionSig { get; set; }

    /// <summary>
    /// The date and time of the HTTP transmission. Contained in the `PAYPAL-TRANSMISSION-TIME` header of the notification message.
    /// </summary>
    public string TransmissionTime { get; set; }

    /// <summary>
    /// The ID of the webhook as configured in your Developer Portal account.
    /// </summary>
    public string WebhookId { get; set; }

    /// <summary>
    /// The webhook notification, which is the content of the HTTP `POST` request body.
    /// </summary>
    public WebhookEvent WebhookEvent { get; set; }

    /// <summary>Verifies a webhook signature.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>VerifyWebhookSignatureResponse</returns>
    public VerifyWebhookSignatureResponse Post(IPayPalClient apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/notifications/verify-webhook-signature";
        return ConfigureAndExecute<VerifyWebhookSignatureResponse>(apiContext, HttpMethod.Post, resource, this);
    }
}