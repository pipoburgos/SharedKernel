
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The payout item status and other details.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayoutItemDetails : PayPalRelationalObject
{
    /// <summary>
    /// The ID for the payout item. Viewable when you show details for a batch payout.
    /// </summary>
    public string PayoutItemId { get; set; }

    /// <summary>The PayPal-generated ID for the transaction.</summary>
    public string TransactionId { get; set; }

    /// <summary>The transaction status.</summary>
    //[JsonConverter(typeof (StringEnumConverter))]
    public PayoutTransactionStatus TransactionStatus { get; set; }

    /// <summary>The amount of money, in U.S. dollars, for fees.</summary>
    public PayPalCurrency PayoutItemFee { get; set; }

    /// <summary>The PayPal-generated ID for the batch payout.</summary>
    public string PayoutBatchId { get; set; }

    /// <summary>
    /// A sender-specified ID number. Tracks the batch payout in an accounting system.
    /// </summary>
    public string SenderBatchId { get; set; }

    /// <summary>The sender-provided information for the payout item.</summary>
    public PayoutItem PayoutItem { get; set; }

    /// <summary>The date and time when this item was last processed.</summary>
    public string TimeProcessed { get; set; }

    /// <summary>Error information associated with this item, if any.</summary>
    public Error Error { get; set; }

    /// <summary>
    /// Obtain the status of a payout item by passing the item ID to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="payoutItemId">Payouts generated payout_item_id to obtain status.</param>
    /// <returns>PayoutItemDetails</returns>
    [Obsolete("This method has been moved to the PayoutItem class.", false)]
    public static PayoutItemDetails Get(APIContext apiContext, string payoutItemId)
    {
        return PayoutItem.Get(apiContext, payoutItemId);
    }
}