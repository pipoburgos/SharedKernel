
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Invoicing refund information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class RefundDetail //: PayPalSerializableObject
{
    /// <summary>
    /// The PayPal refund type. Indicates whether refund was paid through PayPal or externally in invoicing flow. The [record refund](/docs/api/invoicing/#invoices_record-refund) method supports the `EXTERNAL` refund type. The `PAYPAL` refund type is supported for backward compatibility.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The ID of the PayPal refund transaction. Required with the `PAYPAL` refund type.
    /// </summary>
    public string TransactionId { get; set; }

    /// <summary>
    /// The date when the invoice was refunded. The date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6). For example, `2014-02-27 PST`.
    /// </summary>
    public string Date { get; set; }

    /// <summary>A note associated with the refund.</summary>
    public string Note { get; set; }

    /// <summary>
    /// The amount to record as refunded. If you omit the amount, the total invoice paid amount is recorded as refunded.
    /// </summary>
    public PayPalCurrency Amount { get; set; }
}