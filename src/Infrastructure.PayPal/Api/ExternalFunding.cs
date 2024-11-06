
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing an external funding object.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ExternalFunding //: PayPalSerializableObject
{
    /// <summary>Unique identifier for the external funding</summary>
    public string ReferenceId { get; set; }

    /// <summary>Generic identifier for the external funding</summary>
    public string Code { get; set; }

    /// <summary>
    /// Encrypted PayPal Account identifier for the funding account
    /// </summary>
    public string FundingAccountId { get; set; }

    /// <summary>Description of the external funding being applied</summary>
    public string DisplayText { get; set; }

    /// <summary>Amount being funded by the external funding account</summary>
    public PayPalCurrency Amount { get; set; }

    /// <summary>
    /// Indicates that the Payment should be fully funded by External Funded Incentive
    /// </summary>
    public string FundingInstruction { get; set; }
}