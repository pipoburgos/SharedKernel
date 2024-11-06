
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The PayPal-generated batch status.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayoutBatch : PayPalRelationalObject
{
    /// <summary>A batch header. Includes the generated batch status.</summary>
    public PayoutBatchHeader BatchHeader { get; set; }

    /// <summary>An array of items in a batch payout.</summary>
    public List<PayoutItemDetails> Items { get; set; }
}