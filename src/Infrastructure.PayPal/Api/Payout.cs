using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A REST API payout resource object.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Payout : PayPalRelationalObject
{
    /// <summary>
    /// The original batch header as provided by the payment sender.
    /// </summary>
    public PayoutSenderBatchHeader SenderBatchHeader { get; set; }

    /// <summary>
    /// An array of payout items (that is, a set of individual payouts).
    /// </summary>
    public List<PayoutItem> Items { get; set; }

    /// <summary>
    /// Create a payout batch resource by passing a sender_batch_header and an items array to the request URI. The sender_batch_header contains payout parameters that describe the handling of a batch resource while the items array conatins payout items.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="syncMode">A value of true will provide an immediate, synchronous response. Without this query keyword or if the value is false, the response will be a background batch mode.</param>
    /// <returns>PayoutCreateResponse</returns>
    public PayoutBatch Create(IPayPalClient apiContext, bool syncMode = false)
    {
        return Create(apiContext, this, syncMode);
    }

    /// <summary>
    /// Create a payout batch resource by passing a sender_batch_header and an items array to the request URI. The sender_batch_header contains payout parameters that describe the handling of a batch resource while the items array conatins payout items.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="payout">Payout object to be created as a PayPal resource.</param>
    /// <param name="syncMode">A value of true will provide an immediate, synchronous response. Without this query keyword or if the value is false, the response will be a background batch mode.</param>
    /// <returns>PayoutCreateResponse</returns>
    public static PayoutBatch Create(IPayPalClient apiContext, Payout payout, bool syncMode = false)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(syncMode, nameof(syncMode));
        var queryParameters = new QueryParameters
        {
            ["sync_mode"] = syncMode.ToString(),
        };
        var resource = "v1/payments/payouts" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<PayoutBatch>(apiContext, HttpMethod.Post, resource, payout);
    }

    /// <summary>
    /// Obtain the status of a specific batch resource by passing the payout batch ID to the request URI. You can issue this call multiple times to get the current status.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="payoutBatchId">Identifier of the Payout Resource to obtain data.</param>
    /// <returns>PayoutBatchStatus</returns>
    public static PayoutBatch Get(IPayPalClient apiContext, string payoutBatchId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(payoutBatchId, nameof(payoutBatchId));
        var resource = SdkUtil.FormatUriPath("v1/payments/payouts/{0}", [
            payoutBatchId,
        ]);
        return ConfigureAndExecute<PayoutBatch>(apiContext, HttpMethod.Get, resource);
    }
}