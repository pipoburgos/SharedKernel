namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// payment amount with break-ups.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Amount //: PayPalSerializableObject
{
    /// <summary>
    /// 3-letter [currency code](https://developer.paypal.com/docs/integration/direct/rest_api_payment_country_currency_support/). PayPal does not support all currencies.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Total amount charged from the payer to the payee. In case of a refund, this is the refunded amount to the original payer from the payee. 10 characters max with support for 2 decimal places.
    /// </summary>
    public string Total { get; set; }

    /// <summary>Additional details of the payment amount.</summary>
    public Details Details { get; set; }
}