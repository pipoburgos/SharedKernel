
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// The verify webhook signature response.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class VerifyWebhookSignatureResponse //: PayPalSerializableObject
{
    /// <summary>
    /// The status of the signature verification. Value is `SUCCESS` or `FAILURE`.
    /// </summary>
    public string VerificationStatus { get; set; }
}