namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Status of a payout transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/payments.payouts-batch#definition-payout_enumerations">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public enum PayoutTransactionStatus
{
    /// <summary>The item has been successfully processed.</summary>
    Success,
    /// <summary>The item has been denied payment.</summary>
    Denied,
    /// <summary>The item is awaiting payment.</summary>
    Pending,
    /// <summary>
    /// The payment processing is delayed due to PayPal internal updates.
    /// </summary>
    New,
    /// <summary>Processing failed for the item.</summary>
    Failed,
    /// <summary>
    /// The item is unclaimed. If the item is not claimed within 30 days, the funds will be returned to the sender.
    /// </summary>
    Unclaimed,
    /// <summary>
    /// The item is returned. The funds are returned if the recipient hasn't claimed them in 30 days.
    /// </summary>
    Returned,
    /// <summary>The item is on hold.</summary>
    Onhold,
    /// <summary>The item is blocked.</summary>
    Blocked,
    /// <summary>
    /// The payment for the payout item was successfully refunded.
    /// </summary>
    Refunded,
}