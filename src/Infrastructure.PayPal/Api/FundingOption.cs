
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// specifies the funding option details.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FundingOption : PayPalRelationalObject
{
    /// <summary>id of the funding option.</summary>
    public string Id { get; set; }

    /// <summary>
    /// List of funding sources that contributes to a payment.
    /// </summary>
    public List<FundingSource> FundingSources { get; set; }

    /// <summary>
    /// Backup funding instrument which will be used for payment if primary fails.
    /// </summary>
    public FundingInstrument BackupFundingInstrument { get; set; }

    /// <summary>
    /// Currency conversion applicable to this funding option.
    /// </summary>
    public CurrencyConversion CurrencyConversion { get; set; }

    /// <summary>Installment options available for a funding option.</summary>
    public InstallmentInfo InstallmentInfo { get; set; }
}