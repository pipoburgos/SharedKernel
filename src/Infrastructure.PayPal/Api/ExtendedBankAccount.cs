
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a bank account that can be used to fund a payment including support for SEPA.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ExtendedBankAccount : BankAccount
{
    /// <summary>
    /// Identifier of the direct debit mandate to validate. Currently supported only for EU bank accounts(SEPA).
    /// </summary>
    public string MandateReferenceNumber { get; set; }
}