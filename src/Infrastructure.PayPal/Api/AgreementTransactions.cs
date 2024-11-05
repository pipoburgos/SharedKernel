namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A list of transactions associated with a billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class AgreementTransactions //: PayPalSerializableObject
{
    /// <summary>Array of agreement_transaction object.</summary>
    public List<AgreementTransaction> AgreementTransactionList { get; set; }
}