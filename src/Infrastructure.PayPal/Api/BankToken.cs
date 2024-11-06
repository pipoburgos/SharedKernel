namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a bank that can be used to fund a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class BankToken //: PayPalSerializableObject
{
    /// <summary>
    /// ID of a previously saved Bank resource using /vault/bank API.
    /// </summary>

    public string BankId { get; set; }

    /// <summary>
    /// The unique identifier of the payer used when saving this bank using /vault/bank API.
    /// </summary>
    public string ExternalCustomerId { get; set; }

    /// <summary>
    /// Identifier of the direct debit mandate to validate. Currently supported only for EU bank accounts(SEPA).
    /// </summary>
    public string MandateReferenceNumber { get; set; }
}