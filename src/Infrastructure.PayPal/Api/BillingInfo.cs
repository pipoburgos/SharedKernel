
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Billing information for the invoice recipient.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class BillingInfo //: PayPalSerializableObject
{
    /// <summary>
    /// The invoice recipient email address.<blockquote><strong>Note:</strong>Before you get a QR code, you must create an invoice that specifies `qrinvoice@paypal.com `as the recipient email address in the `billing_info` object. Use a customer email address only if you want to email the invoice.</blockquote>
    /// </summary>
    public string Email { get; set; }

    /// <summary>The invoice recipient first name.</summary>
    public string FirstName { get; set; }

    /// <summary>The invoice recipient last name.</summary>
    public string LastName { get; set; }

    /// <summary>The invoice recipient company business name.</summary>
    public string BusinessName { get; set; }

    /// <summary>The invoice recipient address.</summary>
    public InvoiceAddress Address { get; set; }

    /// <summary>
    /// The language in which to send the email to the recipient. Used only when the recipient lacks a PayPal account.
    /// </summary>
    public string Language { get; set; }

    /// <summary>Additional information, such as business hours.</summary>
    public string AdditionalInfo { get; set; }

    /// <summary>
    /// The preferred notification channel for the recipient. Value is `SMS` or `EMAIL`. Default is `EMAIL`. If `SMS` is specified, a `phone` value is required.
    /// </summary>
    public string NotificationChannel { get; set; }

    /// <summary>
    /// The mobile phone number to which to send SMS notification if `notification_channel` is `SMS`.
    /// </summary>
    public Phone Phone { get; set; }
}