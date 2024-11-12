
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Recipient bank Details.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class RecipientBankingInstruction //: PayPalSerializableObject
{
    /// <summary>Name of the financial institution.</summary>
    public string? BankName { get; set; }

    /// <summary>Name of the account holder</summary>
    public string? AccountHolderName { get; set; }

    /// <summary>bank account number</summary>
    public string? AccountNumber { get; set; }

    /// <summary>bank routing number</summary>
    public string? RoutingNumber { get; set; }

    /// <summary>IBAN equivalent of the bank</summary>
    public string? InternationalBankAccountNumber { get; set; }

    /// <summary>BIC identifier of the financial institution</summary>
    public string? BankIdentifierCode { get; set; }
}