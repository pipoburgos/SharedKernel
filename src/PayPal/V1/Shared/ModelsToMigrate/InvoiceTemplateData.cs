
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Detailed template information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceTemplateData //: PayPalSerializableObject
{
    /// <summary>Information about the merchant who sends the invoice.</summary>
    public MerchantInfo MerchantInfo { get; set; }

    /// <summary>
    /// The required invoice recipient email address and any optional billing information. Supports one recipient only.
    /// </summary>
    public List<BillingInfo> BillingInfo { get; set; }

    /// <summary>
    /// For invoices sent by email, one or more email addresses to which to send a Cc: copy of the notification. Supports only email addresses under participant.
    /// </summary>
    public List<string> CcInfo { get; set; }

    /// <summary>
    /// The shipping information for entities to whom items are shipped.
    /// </summary>
    public ShippingInfo ShippingInfo { get; set; }

    /// <summary>
    /// The list of items to include in the invoice. Each invoice can contain up to 100 items.
    /// </summary>
    public List<InvoiceItem> Items { get; set; }

    /// <summary>
    /// Optional. The payment deadline for the invoice. Valid value is either `term_type` or `due_date` but not both.
    /// </summary>
    public PaymentTerm PaymentTerm { get; set; }

    /// <summary>
    /// Reference data, such as PO number, to add to the invoice.
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// The invoice level discount, as a percent or an amount value.
    /// </summary>
    public Cost Discount { get; set; }

    /// <summary>The shipping cost, as a percent or an amount value.</summary>
    public ShippingCost ShippingCost { get; set; }

    /// <summary>
    /// The custom amount to apply to an invoice. If you include a label, you must include the custom amount.
    /// </summary>
    public CustomAmount Custom { get; set; }

    /// <summary>
    /// Indicates whether the invoice allows a partial payment. If `false`, invoice must be paid in full. If `true`, the invoice allows partial payments. Default is `false`.
    /// </summary>
    public bool? AllowPartialPayment { get; set; }

    /// <summary>
    /// The minimum amount allowed for a partial payment. Valid if `allow_partial_payment` is `true`.
    /// </summary>
    public PayPalCurrency MinimumAmountDue { get; set; }

    /// <summary>
    /// Indicates whether the invoice allows a partial payment. If `false`, invoice must be paid in full. If `true`, the invoice allows partial payments. Default is `false`.
    /// </summary>
    public bool? TaxCalculatedAfterDiscount { get; set; }

    /// <summary>
    /// Indicates whether the unit price includes tax. Default is `false`.
    /// </summary>
    public bool? TaxInclusive { get; set; }

    /// <summary>The general terms of the invoice.</summary>
    public string Terms { get; set; }

    /// <summary>A note to the payer.</summary>
    public string Note { get; set; }

    /// <summary>A private bookkeeping memo for the merchant.</summary>
    public string MerchantMemo { get; set; }

    /// <summary>The full URL of an external logo image.</summary>
    public string LogoUrl { get; set; }

    /// <summary>The total amount of the invoice.</summary>
    public PayPalCurrency TotalAmount { get; set; }

    /// <summary>List of files that are attached to the invoice.</summary>
    public List<FileAttachment> Attachments { get; set; }
}