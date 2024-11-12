namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Details of a transaction associated with a billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class AgreementTransaction //: PayPalSerializableObject
{
    /// <summary>Id corresponding to this transaction.</summary>
    public string? TransactionId { get; set; }

    /// <summary>State of the subscription at this time.</summary>
    public string? Status { get; set; }

    /// <summary>Type of transaction, usually Recurring Payment.</summary>
    public string? TransactionType { get; set; }

    /// <summary>Amount for this transaction.</summary>
    public PayPalCurrency? Amount { get; set; }

    /// <summary>Fee amount for this transaction.</summary>
    public PayPalCurrency? FeeAmount { get; set; }

    /// <summary>Net amount for this transaction.</summary>
    public PayPalCurrency? NetAmount { get; set; }

    /// <summary>Email id of payer.</summary>
    public string? PayerEmail { get; set; }

    /// <summary>Business name of payer.</summary>
    public string? PayerName { get; set; }

    /// <summary>Time at which this transaction happened.</summary>
    public string? TimeStamp { get; set; }

    /// <summary>Time zone of time_updated field.</summary>
    public string? TimeZone { get; set; }
}