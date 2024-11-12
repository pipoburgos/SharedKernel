namespace PayPal.V1.Payments;

/// <summary>
/// A resource representing a Payer's funding instrument. An instance of this schema is valid if and only if it is valid against exactly one of these supported properties
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FundingInstrument
{
    /// <summary>Credit Card instrument.</summary>
    public CreditCard? CreditCard { get; set; }

    /// <summary>PayPal vaulted credit Card instrument.</summary>
    public CreditCardToken? CreditCardToken { get; set; }

    /// <summary>
    /// Payment Card information.
    /// <para>NOTE: This property is currently not supported as a funding instrument option with the PayPal REST API.</para>
    /// </summary>
    public PaymentCard? PaymentCard { get; set; }
}