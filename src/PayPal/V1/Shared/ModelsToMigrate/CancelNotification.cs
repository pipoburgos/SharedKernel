
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Cancels an email or SMS notification.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CancelNotification //: PayPalSerializableObject
{
    /// <summary>The subject of the notification.</summary>
    public string? Subject { get; set; }

    /// <summary>A note to the payer.</summary>
    public string? Note { get; set; }

    /// <summary>
    /// Indicates whether to send the notification to the merchant.
    /// </summary>
    public bool? SendToMerchant { get; set; }

    /// <summary>
    /// Indicates whether to send the notification to the payer.
    /// </summary>
    public bool? SendToPayer { get; set; }

    /// <summary>
    /// An array of one or more Cc: emails. If you omit this parameter from the JSON request body, a notification is sent to all Cc: email addresses that are part of the invoice. Otherwise, specify this parameter to limit the email addresses to which a notification is sent.<blockquote><strong>Note:</strong> Additional email addresses are not supported.</blockquote>
    /// </summary>
    public List<string>? CcEmails { get; set; }
}