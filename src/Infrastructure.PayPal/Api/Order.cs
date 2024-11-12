using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// An order transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Order : PayPalRelationalObject
{
    /// <summary>Identifier of the order transaction.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Identifier to the purchase unit associated with this object. Obsolete. Use one in cart_base.
    /// </summary>
    public string PurchaseUnitReferenceId { get; set; }

    /// <summary>Amount being collected.</summary>
    public Amount Amount { get; set; }

    /// <summary>specifies payment mode of the transaction</summary>
    public string PaymentMode { get; set; }

    /// <summary>State of the order transaction.</summary>
    public string State { get; set; }

    /// <summary>
    /// Reason code for the transaction state being Pending or Reversed. This field will replace pending_reason field eventually. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    /// [DEPRECATED] Reason code for the transaction state being Pending. Obsolete. Retained for backward compatability. Use reason_code field above instead.
    /// </summary>
    public string PendingReason { get; set; }

    /// <summary>
    /// The level of seller protection in force for the transaction.
    /// </summary>
    public string ProtectionEligibility { get; set; }
    public string ProtectionEligibilityType { get; set; }

    /// <summary>
    /// ID of the Payment resource that this transaction is based on.
    /// </summary>
    public string ParentPayment { get; set; }

    /// <summary>
    /// Fraud Management Filter (FMF) details applied for the payment that could result in accept/deny/pending action.
    /// </summary>
    public FmfDetails FmfDetails { get; set; }

    /// <summary>Time the resource was created in UTC ISO8601 format.</summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// Time the resource was last updated in UTC ISO8601 format.
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>Shows details for an order, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="orderId">The ID of the order for which to show details.</param>
    /// <returns>Order</returns>
    public static Order Get(IPayPalClient apiContext, string orderId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(orderId, nameof(orderId));
        var resource = SdkUtil.FormatUriPath("v1/payments/orders/{0}", [
            orderId,
        ]);
        return ConfigureAndExecute<Order>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Captures a payment for an order, by ID. To use this call, the original payment call must specify an intent of `order`. In the JSON request body, include the payment amount and indicate whether this capture is the final capture for the authorization.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="capture">Capture</param>
    /// <returns>Capture</returns>
    public Capture Capture(IPayPalClient apiContext, Capture capture)
    {
        return Capture(apiContext, Id, capture);
    }

    /// <summary>
    /// Creates (and processes) a new Capture Transaction added as a related resource.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="orderId">ID of the order to capture.</param>
    /// <param name="capture">Capture</param>
    /// <returns>Capture</returns>
    public static Capture Capture(
        IPayPalClient apiContext,
        string orderId,
        Capture capture)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(orderId, nameof(orderId));
        ArgumentValidator.Validate(capture, nameof(capture));
        var resource = SdkUtil.FormatUriPath("v1/payments/orders/{0}/capture", [
            orderId,
        ]);
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, capture);
    }

    /// <summary>
    /// Voids, or cancels, an order, by ID. You cannot void an order if a payment has already been partially or fully captured.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Order</returns>
    public Order Void(IPayPalClient apiContext)
    {
        return Void(apiContext, Id);
    }

    /// <summary>Voids (cancels) an Order.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="orderId">ID of the order to void.</param>
    /// <returns>Order</returns>
    public static Order Void(IPayPalClient apiContext, string orderId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(orderId, nameof(orderId));
        var resource = SdkUtil.FormatUriPath("v1/payments/orders/{0}/do-void", [
            orderId,
        ]);
        return ConfigureAndExecute<Order>(apiContext, HttpMethod.Post, resource);
    }

    /// <summary>
    /// Authorizes an order, by ID. Include an `amount` object in the JSON request body.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Authorization</returns>
    public Authorization Authorize(IPayPalClient apiContext)
    {
        return Authorize(apiContext, this);
    }

    /// <summary>Creates an authorization on an order</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="order">Order object to authorize.</param>
    /// <returns>Authorization</returns>
    public static Authorization Authorize(IPayPalClient apiContext, Order order)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(order, nameof(order));
        var resource = SdkUtil.FormatUriPath("v1/payments/orders/{0}/authorize", [
            order.Id,
        ]);
        return ConfigureAndExecute<Authorization>(apiContext, HttpMethod.Post, resource, order);
    }
}