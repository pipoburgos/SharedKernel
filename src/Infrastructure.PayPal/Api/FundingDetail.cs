
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Additional detail of the funding.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FundingDetail //: PayPalSerializableObject
{
    /// <summary>Expected clearing time</summary>
    public string ClearingTime { get; set; }

    /// <summary>
    /// [DEPRECATED] Hold-off duration of the payment. payment_debit_date should be used instead.
    /// </summary>
    public string PaymentHoldDate { get; set; }

    /// <summary>
    /// Date when funds will be debited from the payer's account
    /// </summary>
    public string PaymentDebitDate { get; set; }

    /// <summary>Processing type of the payment card</summary>
    public string ProcessingType { get; set; }
}