namespace PayPal.V1.Payments;

/// <summary>
///  A resource describing an installment
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InstallmentOption //: PayPalSerializableObject
{
    /// <summary>Number of installments</summary>
    public int Term { get; set; }

    /// <summary>Monthly payment</summary>
    public PayPalCurrency? MonthlyPayment { get; set; }

    /// <summary>Discount amount applied to the payment, if any</summary>
    public PayPalCurrency? DiscountAmount { get; set; }

    /// <summary>Discount percentage applied to the payment, if any</summary>
    public string? DiscountPercentage { get; set; }
}