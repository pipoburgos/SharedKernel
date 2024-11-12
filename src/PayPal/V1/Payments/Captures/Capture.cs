using PayPal.V1.Shared;
using PayPal.V1.Shared.ModelsToMigrate;
using PayPal.V1.Shared.Util;

/* Unmerged change from project 'SharedKernel.Infrastructure.PayPal (net6.0)'
Before:
using SharedKernel.Infrastructure.PayPal.V1.Shared;
After:
using SharedKernel.Infrastructure.PayPal.V1;
using SharedKernel.Infrastructure.PayPal.V1.Payments;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture.Capture.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Shared;
*/

/* Unmerged change from project 'SharedKernel.Infrastructure.PayPal (net7.0)'
Before:
using SharedKernel.Infrastructure.PayPal.V1.Shared;
After:
using SharedKernel.Infrastructure.PayPal.V1;
using SharedKernel.Infrastructure.PayPal.V1.Payments;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Shared;
*/

/* Unmerged change from project 'SharedKernel.Infrastructure.PayPal (net8.0)'
Before:
using SharedKernel.Infrastructure.PayPal.V1.Shared;
After:
using SharedKernel.Infrastructure.PayPal.V1;
using SharedKernel.Infrastructure.PayPal.V1.Payments;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Payments.Capture.Capture;
using SharedKernel.Infrastructure.PayPal.V1.Shared;
*/

namespace PayPal.V1.Payments.Captures;

/// <summary>
/// A capture transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Capture : PayPalRelationalObject
{
    /// <summary>The ID of the capture transaction.</summary>
    public string? Id { get; set; }

    /// <summary>
    /// The amount to capture. If the amount matches the orginally authorized amount, the state of the authorization changes to `captured`. If not, the state of the authorization changes to `partially_captured`.
    /// </summary>
    public Amount? Amount { get; set; }

    /// <summary>
    /// Indicates whether to release all remaining funds that the authorization holds in the funding instrument. Default is `false`.
    /// </summary>
    public bool? IsFinalCapture { get; set; }

    /// <summary>The state of the capture.</summary>
    public string? State { get; set; }

    /// <summary>
    /// The reason code that describes why the transaction state is pending or reversed.
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// The ID of the payment on which this transaction is based.
    /// </summary>
    public string? ParentPayment { get; set; }

    /// <summary>The invoice number to track this payment.</summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>The transaction fee for this payment.</summary>
    public PayPalCurrency? TransactionFee { get; set; }

    /// <summary>
    /// The date and time of capture, as defined in [RFC 3339 Section 5.6](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string? CreateTime { get; set; }

    /// <summary>The date and time when the resource was last updated.</summary>
    public string? UpdateTime { get; set; }

    /// <summary>Shows details for a captured payment, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="captureId">The ID of the captured payment for which to show details.</param>
    /// <returns>Capture</returns>
    public static Capture Get(IPayPalClient apiContext, string captureId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(captureId, nameof(captureId));
        var resource = SdkUtil.FormatUriPath("v1/payments/capture/{0}", [
            captureId,
        ]);
        return ConfigureAndExecute<Capture>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Refunds a captured payment, by ID. Include an `amount` object in the JSON request body.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="captureId"></param>
    /// <param name="refundRequest">RefundRequest</param>
    /// <returns>DetailedRefund</returns>
    public static DetailedRefund Refund(
        IPayPalClient apiContext,
        string captureId,
        RefundRequest refundRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(captureId, nameof(captureId));
        ArgumentValidator.Validate(refundRequest, nameof(refundRequest));
        var resource = SdkUtil.FormatUriPath("v1/payments/capture/{0}/refund", [
            captureId,
        ]);
        return ConfigureAndExecute<DetailedRefund>(apiContext, HttpMethod.Post, resource, refundRequest);
    }
}