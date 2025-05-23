﻿
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Invoicing payment information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentDetail //: PayPalSerializableObject
{
    /// <summary>
    /// The payment type in an invoicing flow. Value is `PAYPAL` or `EXTERNAL`. The [record refund](/docs/api/invoicing/#invoices_record-refund) method supports the `EXTERNAL` refund type. The `PAYPAL` refund type is supported for backward compatibility.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The ID for a PayPal payment transaction. Required with the `PAYPAL` payment type.
    /// </summary>
    public string TransactionId { get; set; }

    /// <summary>
    /// The transaction type. Value is `SALE`, `AUTHORIZATION`, or `CAPTURE`.
    /// </summary>
    public string TransactionType { get; set; }

    /// <summary>
    /// The date when the invoice was paid. The date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// The payment mode or method. Required with the `EXTERNAL` payment type. Value is bank transfer, cash, check, credit card, debit card, PayPal, wire transfer, or other.
    /// </summary>
    public string Method { get; set; }

    /// <summary>Optional. A note associated with the payment.</summary>
    public string Note { get; set; }

    /// <summary>
    /// The payment amount to record against the invoice. If you omit this parameter, records the total invoice amount as paid.
    /// </summary>
    public PayPalCurrency Amount { get; set; }
}