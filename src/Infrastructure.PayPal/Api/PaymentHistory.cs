
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// List of Payments made by the seller.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentHistory //: PayPalSerializableObject
{
    /// <summary>A list of Payment resources</summary>
    public List<Payment> Payments { get; set; }

    /// <summary>
    /// Number of items returned in each range of results. Note that the last results range could have fewer items than the requested number of items. Maximum value: 20.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Identifier of the next element to get the next range of results.
    /// </summary>
    public string NextId { get; set; }
}