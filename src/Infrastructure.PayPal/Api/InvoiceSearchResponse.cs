
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Response object from an invoice search operation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceSearchResponse //: PayPalSerializableObject
{
    /// <summary>Total number of invoices.</summary>
    public int TotalCount { get; set; }

    /// <summary>List of invoices belonging to a merchant.</summary>
    public List<Invoice> Invoices { get; set; }
}