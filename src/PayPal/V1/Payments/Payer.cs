namespace PayPal.V1.Payments;

/// <summary>
/// A resource representing a Payer that funds a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Payer //: PayPalSerializableObject
{
    /// <summary>
    /// Payment method being used - PayPal Wallet payment, Bank Direct Debit  or Direct Credit card.
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>Status of payer's PayPal Account.</summary>
    public string? Status { get; set; }

    /// <summary>Type of account relationship payer has with PayPal.</summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Duration since the payer established account relationship with PayPal in days.
    /// </summary>
    public string? AccountAge { get; set; }

    /// <summary>
    /// List of funding instruments to fund the payment. 'OneOf' funding_instruments,funding_option_id to be used to identify the specifics of payment method passed.
    /// </summary>
    public List<FundingInstrument>? FundingInstruments { get; set; }

    /// <summary>
    /// Instrument type pre-selected by the user outside of PayPal and passed along the payment creation. This param is used in cases such as PayPal Credit Second Button
    /// </summary>
    public string? FundingOptionId { get; set; }

    /// <summary>Default funding option available for the payment</summary>
    public FundingOption? FundingOption { get; set; }

    /// <summary>Funding option related to default funding option.</summary>
    public FundingOption? RelatedFundingOption { get; set; }

    /// <summary>Information related to the Payer.</summary>
    public PayerInfo? PayerInfo { get; set; }

    /// <summary>
    /// Instrument type pre-selected by the user outside of PayPal and passed along the payment creation. This param is used in cases such as PayPal Credit Second Button
    /// </summary>
    public string? ExternalSelectedFundingInstrumentType { get; set; }
}