
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Merchant preferences for a billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class MerchantPreferences //: PayPalSerializableObject
{
    /// <summary>
    /// Identifier of the merchant_preferences. 128 characters max.
    /// </summary>
    public string Id { get; set; }

    /// <summary>Setup fee amount. Default is 0.</summary>
    public PayPalCurrency SetupFee { get; set; }

    /// <summary>
    /// Redirect URL on cancellation of agreement request. 1000 characters max.
    /// </summary>
    public string CancelUrl { get; set; }

    /// <summary>
    /// Redirect URL on creation of agreement request. 1000 characters max.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// Notify URL on agreement creation. 1000 characters max.
    /// </summary>
    public string NotifyUrl { get; set; }

    /// <summary>
    /// Total number of failed attempts allowed. Default is 0, representing an infinite number of failed attempts.
    /// </summary>
    public string MaxFailAttempts { get; set; }

    /// <summary>
    /// Allow auto billing for the outstanding amount of the agreement in the next cycle. Allowed values: `YES`, `NO`. Default is `NO`.
    /// </summary>
    public string AutoBillAmount { get; set; }

    /// <summary>
    /// Action to take if a failure occurs during initial payment. Allowed values: `CONTINUE`, `CANCEL`. Default is continue.
    /// </summary>
    public string InitialFailAmountAction { get; set; }

    /// <summary>Payment types that are accepted for this plan.</summary>
    public string AcceptedPaymentType { get; set; }

    /// <summary>char_set for this plan.</summary>
    public string CharSet { get; set; }
}