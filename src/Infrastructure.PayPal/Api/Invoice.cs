using SharedKernel.Infrastructure.PayPal.Util;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Detailed invoice information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Invoice : PayPalResource
{
    /// <summary>The ID of the invoice.</summary>
    public string Id { get; set; }

    /// <summary>
    /// The unique invoice number. If you omit this number, it is auto-incremented from the previous invoice number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The ID of the template from which to create the invoice. Useful for copy functionality.
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>The URI of the invoice.</summary>
    public string Uri { get; set; }

    /// <summary>The invoice status.</summary>
    public string Status { get; set; }

    /// <summary>
    /// Additional information about the merchant who sends the invoice.
    /// </summary>
    public MerchantInfo MerchantInfo { get; set; }

    /// <summary>
    /// The required invoice recipient email address and any optional billing information. Supports only one recipient.
    /// </summary>
    public List<BillingInfo> BillingInfo { get; set; }

    /// <summary>
    /// For invoices sent by email, one or more email addresses to which to send a Cc: copy of the notification. Supports only email addresses under participant.
    /// </summary>
    public List<Participant> CcInfo { get; set; }

    /// <summary>
    /// The shipping information for entities to whom items are being shipped.
    /// </summary>
    public ShippingInfo ShippingInfo { get; set; }

    /// <summary>
    /// The items to include in the invoice. An invoice can contain a maximum of 100 items.
    /// </summary>
    public List<InvoiceItem> Items { get; set; }

    /// <summary>
    /// The date when the invoice was enabled. The date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string InvoiceDate { get; set; }

    /// <summary>
    /// Optional. The payment deadline for the invoice. Value is either `term_type` or `due_date` but not both.
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
    /// The custom amount to apply to an invoice. If you include a label, you must include a custom amount.
    /// </summary>
    public CustomAmount Custom { get; set; }

    /// <summary>
    /// Indicates whether the invoice allows a partial payment. If `false`, invoice must be paid in full. If `true`, the invoice allows partial payments. Default is `false`.
    /// </summary>
    public bool? AllowPartialPayment { get; set; }

    /// <summary>
    /// The minimum amount allowed for a partial payment. Required if `allow_partial_payment` is `true`.
    /// </summary>
    public PayPalCurrency MinimumAmountDue { get; set; }

    /// <summary>
    /// Indicates whether the tax is calculated before or after a discount. If `false`, the tax is calculated before a discount. If `true`, the tax is calculated after a discount. Default is `false`.
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

    /// <summary>The full URL to an external logo image.</summary>
    public string LogoUrl { get; set; }

    /// <summary>The total amount of the invoice.</summary>
    public PayPalCurrency TotalAmount { get; set; }

    /// <summary>List of payment details for the invoice.</summary>
    public List<PaymentDetail> Payments { get; set; }

    /// <summary>List of refund details for the invoice.</summary>
    public List<RefundDetail> Refunds { get; set; }

    /// <summary>Audit information for the invoice.</summary>
    public Metadata Metadata { get; set; }

    /// <summary>Any miscellaneous invoice data.</summary>
    public string AdditionalData { get; set; }

    /// <summary>Gratuity to include with the invoice.</summary>
    public PayPalCurrency Gratuity { get; set; }

    /// <summary>
    /// Payment summary of the invoice including amount paid through PayPal and other sources.
    /// </summary>
    [JsonIgnore]
    [Obsolete("This property is obsolete. Use payments instead.", false)]
    public List<PaymentDetail> PaymentDetails
    {
        get => Payments;
        set => Payments = value;
    }

    /// <summary>
    /// Payment summary of the invoice including amount paid through PayPal and other sources.
    /// </summary>
    public PaymentSummary PaidAmount { get; set; }

    /// <summary>
    /// Payment summary of the invoice, including amount refunded through PayPal and other sources.
    /// </summary>
    [JsonIgnore]
    [Obsolete("This property is obsolete. Use refunds instead.", false)]
    public List<RefundDetail> RefundDetails
    {
        get => Refunds;
        set => Refunds = value;
    }

    /// <summary>
    /// Payment summary of the invoice, including amount refunded through PayPal and other sources.
    /// </summary>
    public PaymentSummary RefundedAmount { get; set; }

    /// <summary>List of files that are attached to the invoice.</summary>
    public List<FileAttachment> Attachments { get; set; }

    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Invoice</returns>
    public Invoice Create(APIContext apiContext) => Create(apiContext, this);

    /// <summary>
    /// Creates a draft invoice. You can optionally create an invoice [template](/docs/api/invoicing/#templates). Then, when you create an invoice from a template, the invoice is populated with the predefined data that the source template contains. To move the invoice from a draft to payable state, you must [send the invoice](/docs/api/invoicing/#invoices_send). In the JSON request body, include invoice details including merchant information. The `invoice` object must include an `items` array.<blockquote><strong>Note:</strong> The merchant specified in an invoice must have a PayPal account in good standing.</blockquote>
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoice">Invoice object to be used for creating the PayPal resource.</param>
    /// <returns>Invoice</returns>
    public static Invoice Create(APIContext apiContext, Invoice invoice)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/invoicing/invoices";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, invoice);
    }

    /// <summary>
    /// Lists invoices that match search criteria. In the JSON request body, include a `search` object that specifies the search criteria.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="search">Search</param>
    /// <returns>InvoiceSearchResponse</returns>
    public static InvoiceSearchResponse Search(APIContext apiContext, Search search)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(search, nameof(search));
        const string resource = "v1/invoicing/search";
        return ConfigureAndExecute<InvoiceSearchResponse>(apiContext, HttpMethod.Post, resource, search);
    }

    /// <summary>
    /// Sends an invoice, by ID, to a customer.<blockquote><strong>Note:</strong> After you send an invoice, you cannot resend it.</blockquote><br />Optionally, set the `notify_merchant` query parameter to also send the merchant an invoice update notification. Default is `true`.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="notifyMerchant">Indicates whether to send the invoice update notification to the merchant. Default is `true`.</param>
    public void Send(APIContext apiContext, bool notifyMerchant = true)
    {
        Send(apiContext, Id, notifyMerchant);
    }

    /// <summary>Sends a legitimate invoice to the payer.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">ID of the invoice to send.</param>
    /// <param name="notifyMerchant">Specifies if the invoice send notification is needed for merchant</param>
    public static void Send(APIContext apiContext, string invoiceId, bool notifyMerchant = true)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        var queryParameters = new QueryParameters
        {
            ["notify_merchant"] = notifyMerchant.ToString(),
        };
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/send", [
            invoiceId,
        ]) + queryParameters.ToUrlFormattedString();
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource);
    }

    /// <summary>
    /// Sends a reminder about an invoice, by ID, to a customer. In the JSON request body, include a `notification` object that defines the subject of the reminder and other details.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="notification">Notification</param>
    public void Remind(APIContext apiContext, Notification notification)
    {
        Remind(apiContext, Id, notification);
    }

    /// <summary>Reminds the payer to pay the invoice.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">ID of the invoice the payer will be reminded to pay.</param>
    /// <param name="notification">Notification</param>
    public static void Remind(APIContext apiContext, string invoiceId, Notification notification)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(notification, nameof(notification));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/remind", [
            invoiceId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, notification);
    }

    /// <summary>
    /// Cancels a sent invoice, by ID, and, optionally, sends a notification about the cancellation to the payer, merchant, and Cc: emails.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="cancelNotification">CancelNotification</param>
    public void Cancel(APIContext apiContext, CancelNotification cancelNotification)
    {
        Cancel(apiContext, Id, cancelNotification);
    }

    /// <summary>Cancels an invoice.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">ID of the invoice to cancel.</param>
    /// <param name="cancelNotification">CancelNotification</param>
    public static void Cancel(
        APIContext apiContext,
        string invoiceId,
        CancelNotification cancelNotification)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(cancelNotification, nameof(cancelNotification));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/cancel", [
            invoiceId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, cancelNotification);
    }

    /// <summary>
    /// Marks the status of a specified invoice, by ID, as paid. Include a payment detail object that defines the payment method and other details in the JSON request body.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="paymentDetail">PaymentDetail</param>
    public void RecordPayment(APIContext apiContext, PaymentDetail paymentDetail)
    {
        RecordPayment(apiContext, Id, paymentDetail);
    }

    /// <summary>Mark the status of the invoice as paid.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">ID of the invoice to mark as paid.</param>
    /// <param name="paymentDetail">PaymentDetail</param>
    public static void RecordPayment(
        APIContext apiContext,
        string invoiceId,
        PaymentDetail paymentDetail)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(paymentDetail, nameof(paymentDetail));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/record-payment", [
            invoiceId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, paymentDetail);
    }

    /// <summary>
    /// Marks the status of an invoice, by ID, as refunded. In the JSON request body, include a payment detail object that defines the payment method and other details.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="refundDetail">RefundDetail</param>
    public void RecordRefund(APIContext apiContext, RefundDetail refundDetail)
    {
        RecordRefund(apiContext, Id, refundDetail);
    }

    /// <summary>Mark the status of the invoice as refunded.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">ID fo the invoice to mark as refunded.</param>
    /// <param name="refundDetail">RefundDetail</param>
    public static void RecordRefund(
        APIContext apiContext,
        string invoiceId,
        RefundDetail refundDetail)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(refundDetail, nameof(refundDetail));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/record-refund", [
            invoiceId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, refundDetail);
    }

    /// <summary>Shows details for a specified invoice, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">The ID of the invoice for which to show details.</param>
    /// <returns>Invoice</returns>
    public static Invoice Get(APIContext apiContext, string invoiceId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}", [
            invoiceId,
        ]);
        return ConfigureAndExecute<Invoice>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Lists merchant invoices. Optionally, you can specify one or more query parameters to filter the response.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="page">A *zero-relative* index of the list of merchant invoices.</param>
    /// <param name="pageSize">The number of invoices to list beginning with the specified `page`.</param>
    /// <param name="totalCountRequired">Indicates whether the total count appears in the response. Default is `false`.</param>
    /// <returns>InvoiceSearchResponse</returns>
    public static InvoiceSearchResponse GetAll(
        APIContext apiContext,
        int page = 1,
        int pageSize = 20,
        bool totalCountRequired = false)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            [nameof(page)] = page.ToString(),
            ["page_size"] = pageSize.ToString(),
            ["total_count_required"] = totalCountRequired.ToString(),
        };
        var resource = "v1/invoicing/invoices" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<InvoiceSearchResponse>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Fully updates an invoice, by ID. In the JSON request body, include a complete `invoice` object. This call does not support partial updates.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="notifyMerchant">Indicates whether to send the invoice update notification to the merchant. Default is `true`.</param>
    /// <returns>Invoice</returns>
    public Invoice Update(APIContext apiContext, bool notifyMerchant = true)
    {
        return Update(apiContext, this, notifyMerchant);
    }

    /// <summary>
    /// Full update of the invoice resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoice">Invoice object to update.</param>
    /// <param name="notifyMerchant">Specifies if the invoice update notification is needed for merchant</param>
    /// <returns>Invoice</returns>
    public static Invoice Update(APIContext apiContext, Invoice invoice, bool notifyMerchant = true)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoice, nameof(invoice));
        var queryParameters = new QueryParameters
        {
            ["notify_merchant"] = notifyMerchant.ToString(),
        };
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}", [
            invoice.Id,
        ]) + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute(apiContext, HttpMethod.Put, resource, invoice);
    }

    /// <summary>
    /// Deletes a draft invoice, by ID. Note that this call works for invoices in the draft state only. For invoices that have already been sent, you can [cancel the invoice](/docs/api/invoicing/#invoices_cancel). After you delete a draft invoice, you can no longer use it or show its details. However, you can reuse its invoice number.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Delete(APIContext apiContext) => Delete(apiContext, Id);

    /// <summary>
    /// Deletes a draft invoice, by ID. Note that this call works for invoices in the draft state only. For invoices that have already been sent, you can [cancel the invoice](/docs/api/invoicing/#invoices_cancel). After you delete a draft invoice, you can no longer use it or show its details. However, you can reuse its invoice number.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">The ID of the invoice to delete.</param>
    public static void Delete(APIContext apiContext, string invoiceId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}", [
            invoiceId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>
    /// Deletes an external payment, by invoice ID and transaction ID.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">The ID of the invoice from which to delete a payment transaction.</param>
    /// <param name="transactionId">The ID of the payment transaction to delete.</param>
    public static void DeleteExternalPayment(
        APIContext apiContext,
        string invoiceId,
        string transactionId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(transactionId, nameof(transactionId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/payment-records/{1}", [
            invoiceId,
            transactionId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>
    /// Deletes an external refund, by invoice ID and transaction ID.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">The ID of the invoice from which to delete the refund transaction.</param>
    /// <param name="transactionId">The ID of the refund transaction to delete.</param>
    public static void DeleteExternalRefund(
        APIContext apiContext,
        string invoiceId,
        string transactionId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        ArgumentValidator.Validate(transactionId, nameof(transactionId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/refund-records/{1}", [
            invoiceId,
            transactionId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>
    /// Generates a QR code for an invoice, by ID.<br /><br />The QR code is a PNG image in [Base64-encoded](https://www.base64encode.org/) format that corresponds to the invoice ID. You can generate a QR code for an invoice and add it to a paper or PDF invoice. When a customer uses their mobile device to scan the QR code, he or she is redirected to the PayPal mobile payment flow where he or she can pay online with PayPal or a credit card.<br /><br />Before you get a QR code, you must:<ol><li><p>[Create an invoice](#invoices_create). Specify `qrinvoice@paypal.com` as the recipient email address in the `billing_info` object. Use a customer email address only if you want to email the invoice.</p></li><li><p>[Send an invoice](#invoices_send) to move the invoice from a draft to payable state. If you specify `qrinvoice@paypal.com` as the recipient email address, the invoice is not emailed.</p></li></ol>
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="invoiceId">The ID of the invoice for which to generate a QR code.</param>
    /// <param name="width">The width, in pixels, of the QR code image. Valid value is from 150 to 500. Default is 500.</param>
    /// <param name="height">The height, in pixels, of the QR code image. Valid value is from 150 to 500. Default is 500.</param>
    /// <param name="action">The type of URL for which to generate a QR code. Default is `pay` and is the only supported value.</param>
    /// <returns>Image</returns>
    public static PayPalImage QrCode(
        APIContext apiContext,
        string invoiceId,
        int width = 500,
        int height = 500,
        string action = "pay")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(invoiceId, nameof(invoiceId));
        var queryParameters = new QueryParameters
        {
            [nameof(width)] = width.ToString(),
            [nameof(height)] = height.ToString(),
            [nameof(action)] = action,
        };
        var resource = SdkUtil.FormatUriPath("v1/invoicing/invoices/{0}/qr-code", [
            invoiceId,
        ]) + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<PayPalImage>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Generates the next invoice number that is available to the user.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>InvoiceNumber</returns>
    public InvoiceNumber GenerateNumber(APIContext apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var resource = "v1/invoicing/invoices/next-invoice-number";
        return ConfigureAndExecute<InvoiceNumber>(apiContext, HttpMethod.Post, resource);
    }
}