using PayPal.V1.Shared;

namespace PayPal.V1.Payments;

/// <summary>
/// specifies the funding source details.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FundingSource : PayPalRelationalObject
{
    /// <summary>specifies funding mode of the instrument</summary>
    public string? FundingMode { get; set; }

    /// <summary>Instrument type for this funding source</summary>
    public string? FundingInstrumentType { get; set; }

    /// <summary>
    /// Soft descriptor used when charging this funding source.
    /// </summary>
    public string? SoftDescriptor { get; set; }

    /// <summary>
    /// Total anticipated amount of money to be pulled from instrument.
    /// </summary>
    public PayPalCurrency? Amount { get; set; }

    /// <summary>
    /// Additional amount to be pulled from the instrument to recover a negative balance on the buyer's account that is owed to PayPal.
    /// </summary>
    public PayPalCurrency? NegativeBalanceAmount { get; set; }

    /// <summary>Localized legal text relevant to funding source.</summary>
    public string? LegalText { get; set; }

    /// <summary>URL to legal terms relevant to funding source.</summary>
    public string? Terms { get; set; }

    /// <summary>Additional detail of the funding.</summary>
    public FundingDetail? FundingDetail { get; set; }

    /// <summary>Additional text relevant to funding source.</summary>
    public string? AdditionalText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public FundingInstrument? Extends { get; set; }
}