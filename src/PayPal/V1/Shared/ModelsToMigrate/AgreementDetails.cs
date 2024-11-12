namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// The details of a billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class AgreementDetails //: PayPalSerializableObject
{
    /// <summary>The outstanding balance for this agreement.</summary>
    public PayPalCurrency? OutstandingBalance { get; set; }

    /// <summary>Number of cycles remaining for this agreement.</summary>
    public string? CyclesRemaining { get; set; }

    /// <summary>Number of cycles completed for this agreement.</summary>
    public string? CyclesCompleted { get; set; }

    /// <summary>
    /// The next billing date for this agreement, represented as 2014-02-19T10:00:00Z format.
    /// </summary>
    public string? NextBillingDate { get; set; }

    /// <summary>
    /// Last payment date for this agreement, represented as 2014-06-09T09:42:31Z format.
    /// </summary>
    public string? LastPaymentDate { get; set; }

    /// <summary>Last payment amount for this agreement.</summary>
    public PayPalCurrency? LastPaymentAmount { get; set; }

    /// <summary>
    /// Last payment date for this agreement, represented as 2015-02-19T10:00:00Z format.
    /// </summary>
    public string? FinalPaymentDate { get; set; }

    /// <summary>Total number of failed payments for this agreement.</summary>
    public string? FailedPaymentCount { get; set; }
}