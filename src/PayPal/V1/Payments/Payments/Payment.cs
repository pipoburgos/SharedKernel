using PayPal.Exceptions;
using PayPal.V1.Shared;
using PayPal.V1.Shared.Util;

namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Lets you create, process and manage payments.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Payment : PayPalRelationalObject
{
    /// <summary>Identifier of the payment resource created.</summary>
    public string? Id { get; set; }

    /// <summary>Payment intent.</summary>
    public string? Intent { get; set; }

    /// <summary>
    /// Source of the funds for this payment represented by a PayPal account or a direct credit card.
    /// </summary>
    public Payer? Payer { get; set; }

    /// <summary>.</summary>
    public Payee? Payee { get; set; }

    /// <summary>ID of the cart to execute the payment.</summary>
    public string? Cart { get; set; }

    /// <summary>
    /// Transactional details including the amount and item details.
    /// </summary>
    public List<Transaction>? Transactions { get; set; }

    /// <summary>
    /// Applicable for advanced payments like multi seller payment (MSP) to support partial failures
    /// </summary>
    public List<Error>? FailedTransactions { get; set; }

    /// <summary>A payment instruction resource</summary>
    public PaymentInstruction? PaymentInstruction { get; set; }

    /// <summary>
    /// The state of the payment, authorization, or order transaction. The value is:<ul><li><code>created</code>. The transaction was successfully created.</li><li><code>approved</code>. The buyer approved the transaction.</li><li><code>failed</code>. The transaction request failed.</li></ul>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// PayPal generated identifier for the merchant's payment experience profile. Refer to [this](https://developer.paypal.com/docs/api/#payment-experience) link to create experience profile ID.
    /// </summary>
    public string? ExperienceProfileId { get; set; }

    /// <summary>
    /// free-form field for the use of clients to pass in a message to the payer
    /// </summary>
    public string? NoteToPayer { get; set; }

    /// <summary>
    /// Set of redirect URLs you provide only for PayPal-based payments.
    /// </summary>
    public RedirectUrls? RedirectUrls { get; set; }

    /// <summary>
    /// Failure reason code returned when the payment failed for some valid reasons.
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Payment creation time as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string? CreateTime { get; set; }

    /// <summary>
    /// Payment update time as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string? UpdateTime { get; set; }

    /// <summary>
    /// Collection of PayPal generated billing agreement tokens.
    /// </summary>
    public List<string>? BillingAgreementTokens { get; set; }

    /// <summary>
    /// Get or sets the token found in the approval_url link returned from a call to create this resource.
    /// </summary>
    //[JsonIgnore]
    public string? Token { get; private set; }

    /// <summary>
    /// Creates and processes a payment. In the JSON request body, include a `payment` object with the intent, payer, and transactions. For PayPal payments, include redirect URLs in the `payment` object.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Payment</returns>
    public Payment Create(IPayPalClient apiContext)
    {
        return Create(apiContext, this);
    }

    /// <summary>Creates (and processes) a new Payment Resource.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="payment">Payment object to be used in creating the PayPal resource.</param>
    /// <returns>Payment</returns>
    public static Payment Create(IPayPalClient apiContext, Payment payment)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/payments/payment";
        var payment1 = ConfigureAndExecute(apiContext, "POST", resource, payment);
        payment1.Token = payment1.GetTokenFromApprovalUrl();
        return payment1;
    }

    /// <summary>Shows details for a payment, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="paymentId">The ID of the payment for which to show details.</param>
    /// <returns>Payment</returns>
    public static Payment Get(IPayPalClient apiContext, string paymentId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(paymentId, nameof(paymentId));
        var resource = SdkUtil.FormatUriPath("v1/payments/payment/{0}", [
            paymentId,
        ]);
        return ConfigureAndExecute<Payment>(apiContext, "GET", resource);
    }

    /// <summary>
    /// Partially updates a payment, by ID. You can update the amount, shipping address, invoice ID, and custom data. You cannot use patch after execute has been called.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public void Update(IPayPalClient apiContext, PatchRequest patchRequest)
    {
        if (Id == null)
            throw new ArgumentNullException(nameof(Id));

        Update(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Partially update the Payment resource for the given identifier
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="paymentId">ID of the payment to update.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public static void Update(IPayPalClient apiContext, string paymentId, PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(paymentId, nameof(paymentId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/payments/payment/{0}", [
            paymentId,
        ]);
        ConfigureAndExecute(apiContext, "PATCH", resource, patchRequest);
    }

    /// <summary>
    /// Executes, or completes, a PayPal payment that the payer has approved. You can optionally update selective payment information when you execute a payment.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="paymentExecution">PaymentExecution</param>
    /// <returns>Payment</returns>
    public Payment Execute(IPayPalClient apiContext, PaymentExecution paymentExecution)
    {
        if (Id == null)
            throw new ArgumentNullException(nameof(Id));

        return Execute(apiContext, Id, paymentExecution);
    }

    /// <summary>
    /// Executes the payment (after approved by the Payer) associated with this resource when the payment method is PayPal.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="paymentId">ID of the payment to execute.</param>
    /// <param name="paymentExecution">PaymentExecution</param>
    /// <returns>Payment</returns>
    public static Payment Execute(
        IPayPalClient apiContext,
        string paymentId,
        PaymentExecution paymentExecution)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(paymentId, nameof(paymentId));
        ArgumentValidator.Validate(paymentExecution, nameof(paymentExecution));
        var resource = SdkUtil.FormatUriPath("v1/payments/payment/{0}/execute", [
            paymentId,
        ]);
        return ConfigureAndExecute<Payment>(apiContext, "POST", resource, paymentExecution);
    }

    /// <summary>
    /// List payments that were made to the merchant who issues the request. Payments can be in any state.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="count">The number of items to list in the response.</param>
    /// <param name="startId">The ID of the starting resource in the response. When results are paged, you can use the `next_id` value as the `start_id` to continue with the next set of results.</param>
    /// <param name="startIndex">The start index of the resources to return. Typically used to jump to a specific position in the resource history based on its cart. Example for starting at the second item in a list of results: `?start_index=2`</param>
    /// <param name="startTime">The date and time when the resource was created. Indicates the start of a range of results. Example: `start_time=2013-03-06T11:00:00Z`</param>
    /// <param name="endTime">The date and time when the resource was created. Indicates the end of a range of results.</param>
    /// <param name="startDate">Resource creation date that indicates the start of results.</param>
    /// <param name="endDate">Resource creation date that indicates the end of a range of results.</param>
    /// <param name="payeeEmail">Payee identifier (email) to filter the search results in list operations.</param>
    /// <param name="payeeId">Payee identifier (merchant id) assigned by PayPal to filter the search results in list operations.</param>
    /// <param name="sortBy">Field name that determines sort order of results.</param>
    /// <param name="sortOrder">Specifies if order of results is ascending or descending.</param>
    /// <returns>PaymentHistory</returns>
    public static PaymentHistory List(
        IPayPalClient apiContext,
        int? count = null,
        string startId = "",
        int? startIndex = null,
        string startTime = "",
        string endTime = "",
        string startDate = "",
        string endDate = "",
        string payeeEmail = "",
        string payeeId = "",
        string sortBy = "",
        string sortOrder = "")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            [nameof(count)] = count?.ToString() ?? string.Empty,
            ["start_id"] = startId,
            ["start_index"] = startIndex?.ToString() ?? string.Empty,
            ["start_time"] = startTime,
            ["end_time"] = endTime,
            ["start_date"] = startDate,
            ["end_date"] = endDate,
            ["payee_email"] = payeeEmail,
            ["payee_id"] = payeeId,
            ["sort_by"] = sortBy,
            ["sort_order"] = sortOrder,
        };
        var resource = "v1/payments/payment" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<PaymentHistory>(apiContext, "GET", resource);
    }
}