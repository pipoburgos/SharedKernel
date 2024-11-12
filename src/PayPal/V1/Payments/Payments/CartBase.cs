
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Base properties of a cart resource
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CartBase //: PayPalSerializableObject
{
    /// <summary>
    /// Merchant identifier to the purchase unit. Optional parameter
    /// </summary>
    public string? ReferenceId { get; set; }

    /// <summary>Amount being collected.</summary>
    public Amount? Amount { get; set; }

    /// <summary>Recipient of the funds in this transaction.</summary>
    public Payee? Payee { get; set; }

    /// <summary>Description of what is being paid for.</summary>
    public string? Description { get; set; }

    /// <summary>
    /// Note to the recipient of the funds in this transaction.
    /// </summary>
    public string? NoteToPayee { get; set; }

    /// <summary>free-form field for the use of clients</summary>
    public string? Custom { get; set; }

    /// <summary>invoice number to track this payment</summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Soft descriptor used when charging this funding source. If length exceeds max length, the value will be truncated
    /// </summary>
    public string? SoftDescriptor { get; set; }

    /// <summary>Payment options requested for this purchase unit</summary>
    public PaymentOptions? PaymentOptions { get; set; }

    /// <summary>List of items being paid for.</summary>
    public ItemList? ItemList { get; set; }

    /// <summary>URL to send payment notifications</summary>
    public string? NotifyUrl { get; set; }

    /// <summary>Url on merchant site pertaining to this payment.</summary>
    public string? OrderUrl { get; set; }
}