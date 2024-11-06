
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Additional details of the payment amount.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Details //: PayPalSerializableObject
{
    /// <summary>
    /// Amount of the subtotal of the items. **Required** if line items are specified. 10 characters max, with support for 2 decimal places.
    /// </summary>
    public string Subtotal { get; set; }

    /// <summary>
    /// Amount charged for shipping. 10 characters max with support for 2 decimal places.
    /// </summary>
    public string Shipping { get; set; }

    /// <summary>
    /// Amount charged for tax. 10 characters max with support for 2 decimal places.
    /// </summary>
    public string Tax { get; set; }

    /// <summary>
    /// Amount being charged for the handling fee. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string HandlingFee { get; set; }

    /// <summary>
    /// Amount being discounted for the shipping fee. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string ShippingDiscount { get; set; }

    /// <summary>
    /// Amount being charged for the insurance fee. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string Insurance { get; set; }

    /// <summary>Amount being charged as gift wrap fee.</summary>
    public string GiftWrap { get; set; }

    /// <summary>
    /// Fee charged by PayPal. In case of a refund, this is the fee amount refunded to the original receipient of the payment.
    /// </summary>
    public string Fee { get; set; }
}