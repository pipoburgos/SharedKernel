
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A refund transaction.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class RefundRequest //: PayPalSerializableObject
{
    /// <summary>
    /// Details including both refunded amount (to payer) and refunded fee (to payee).
    /// </summary>
    public Amount Amount { get; set; }

    /// <summary>
    /// Description of what is being refunded for. Character length and limitations: 255 single-byte alphanumeric characters.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Type of PayPal funding source (balance or eCheck) that can be used for auto refund.
    /// </summary>
    public string RefundSource { get; set; }

    /// <summary>
    /// Reason description for the Sale transaction being refunded.
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// The invoice number that is used to track this payment. Character length and limitations: 127 single-byte alphanumeric characters.
    /// </summary>
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Flag to indicate that the buyer was already given store credit for a given transaction.
    /// </summary>
    public bool? RefundAdvice { get; set; }
}