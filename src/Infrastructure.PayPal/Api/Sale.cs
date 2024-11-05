using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A sale transaction. This is the resource that is returned as a part related resources in Payment
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Sale : PayPalRelationalObject
{
    /// <summary>Identifier of the sale transaction.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Identifier to the purchase or transaction unit corresponding to this sale transaction.
    /// </summary>
    public string PurchaseUnitReferenceId { get; set; }

    /// <summary>Amount being collected.</summary>
    public Amount Amount { get; set; }

    /// <summary>
    /// Specifies payment mode of the transaction. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string PaymentMode { get; set; }

    /// <summary>
    /// your own invoice number or tracking IDs, Maximum length: 127.
    /// </summary>
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// free-form field for the use of clients, Maximum length: 127.
    /// </summary>
    public string Custom { get; set; }

    /// <summary>State of the sale transaction.</summary>
    public string State { get; set; }

    /// <summary>
    /// Reason code for the transaction state being Pending or Reversed. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    /// The level of seller protection in force for the transaction. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ProtectionEligibility { get; set; }

    /// <summary>
    /// The kind of seller protection in force for the transaction. It is returned only when protection_eligibility is ELIGIBLE or PARTIALLY_ELIGIBLE. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ProtectionEligibilityType { get; set; }

    /// <summary>
    /// Expected clearing time for eCheck Transactions. Returned when payment is made with eCheck. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ClearingTime { get; set; }

    /// <summary>
    /// Status of the Recipient Fund. For now, it will be returned only when fund status is held
    /// </summary>
    public string PaymentHoldStatus { get; set; }

    /// <summary>
    /// Reasons for PayPal holding recipient fund. It is set only if payment hold status is held
    /// </summary>
    public List<string> PaymentHoldReasons { get; set; }

    /// <summary>
    /// Indicates the credit status of fund to the recipient. It will be returned only when payment status is 'completed'
    /// </summary>
    [Obsolete("Unused")]
    public string RecipientFundStatus { get; set; }

    /// <summary>Reason for holding the funds.</summary>
    [Obsolete("Unused")]
    public string HoldReason { get; set; }

    /// <summary>Transaction fee applicable for this payment.</summary>
    public PayPalCurrency TransactionFee { get; set; }

    /// <summary>
    /// Net amount the merchant receives for this transaction in their receivable currency. Returned only in cross-currency use cases where a merchant bills a buyer in a non-primary currency for that buyer.
    /// </summary>
    public PayPalCurrency ReceivableAmount { get; set; }

    /// <summary>
    /// Exchange rate applied for this transaction. Returned only in cross-currency use cases where a merchant bills a buyer in a non-primary currency for that buyer.
    /// </summary>
    public string ExchangeRate { get; set; }

    /// <summary>
    /// Fraud Management Filter (FMF) details applied for the payment that could result in accept, deny, or pending action. Returned in a payment response only if the merchant has enabled FMF in the profile settings and one of the fraud filters was triggered based on those settings. See [Fraud Management Filters Summary](/docs/classic/fmf/integration-guide/FMFSummary/) for more information.
    /// </summary>
    public FmfDetails FmfDetails { get; set; }

    /// <summary>
    /// Receipt id is a payment identification number returned for guest users to identify the payment.
    /// </summary>
    public string ReceiptId { get; set; }

    /// <summary>
    /// ID of the payment resource on which this transaction is based.
    /// </summary>
    public string ParentPayment { get; set; }

    /// <summary>
    /// Response codes returned by the processor concerning the submitted payment. Only supported when the `payment_method` is set to `credit_card`.
    /// </summary>
    public ProcessorResponse ProcessorResponse { get; set; }

    /// <summary>
    /// ID of the billing agreement used as reference to execute this transaction.
    /// </summary>
    public string BillingAgreementId { get; set; }

    /// <summary>
    /// Time of sale as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6)
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// Time the resource was last updated in UTC ISO8601 format.
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// Shows details for a sale, by ID. Returns only sales that were created through the REST API.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="saleId">The ID of the sale for which to show details.</param>
    /// <returns>Sale</returns>
    public static Sale Get(APIContext apiContext, string saleId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(saleId, nameof(saleId));
        var resource = SdkUtil.FormatUriPath("v1/payments/sale/{0}", [
            saleId,
        ]);
        return ConfigureAndExecute<Sale>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Creates (and processes) a new Refund Transaction added as a related resource.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="refund">Refund</param>
    /// <returns>Refund</returns>
    [Obsolete("Please use Refund(APIContext, string, RefundRequest) instead")]
    public Refund Refund(APIContext apiContext, Refund refund)
    {
        return Refund(apiContext, Id, refund);
    }

    /// <summary>
    /// Creates (and processes) a new Refund Transaction added as a related resource.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="saleId">ID of the sale to refund.</param>
    /// <param name="refund">Refund</param>
    /// <returns>Refund</returns>
    [Obsolete("Please use Refund(APIContext, string, RefundRequest) instead")]
    public static Refund Refund(APIContext apiContext, string saleId, Refund refund)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(saleId, nameof(saleId));
        ArgumentValidator.Validate(refund, nameof(refund));
        var resource = SdkUtil.FormatUriPath("v1/payments/sale/{0}/refund", [
            saleId,
        ]);
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, refund);
    }

    /// <summary>
    /// Refunds a sale, by ID. For a full refund, include an empty payload in the JSON request body. For a partial refund, include an `amount` object in the JSON request body.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="refundRequest">RefundRequest</param>
    /// <returns>DetailedRefund</returns>
    public static DetailedRefund Refund(
        APIContext apiContext,
        string saleId,
        RefundRequest refundRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(saleId, nameof(saleId));
        ArgumentValidator.Validate(refundRequest, nameof(refundRequest));
        var resource = SdkUtil.FormatUriPath("v1/payments/sale/{0}/refund", [
            saleId,
        ]);
        return ConfigureAndExecute<DetailedRefund>(apiContext, HttpMethod.Post, resource, refundRequest);
    }
}