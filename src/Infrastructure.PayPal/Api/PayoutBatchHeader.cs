
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A batch payout header data object, which can be the response to a batch payout header request. Enables you to get payout header information for an entire batch payout request.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayoutBatchHeader : PayPalRelationalObject
{
    /// <summary>The PayPal-generated ID for a batch payout.</summary>
    public string PayoutBatchId { get; set; }

    /// <summary>
    /// The PayPal-generated batch status. If the batch payout passes preliminary checks, the status is `PENDING`.
    /// </summary>
    public string BatchStatus { get; set; }

    /// <summary>
    /// The date and time when processing for the batch payout began.
    /// </summary>
    public string TimeCreated { get; set; }

    /// <summary>
    /// The date and time when processing for the batch payout completed.
    /// </summary>
    public string TimeCompleted { get; set; }

    /// <summary>
    /// The original batch payout header, as provided by the payment sender.
    /// </summary>
    public PayoutSenderBatchHeader SenderBatchHeader { get; set; }

    /// <summary>
    /// The total amount, in U.S. dollars, requested for the payouts.
    /// </summary>
    public PayPalCurrency Amount { get; set; }

    /// <summary>
    /// The total estimate, in U.S. dollars, for the applicable payouts fees.
    /// </summary>
    public PayPalCurrency Fees { get; set; }

    /// <summary>An array of batch errors, if any.</summary>
    public List<Error> Errors { get; set; }
}