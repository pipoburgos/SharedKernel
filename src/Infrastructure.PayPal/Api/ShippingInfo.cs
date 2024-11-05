
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Shipping information for the invoice recipient.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ShippingInfo //: PayPalSerializableObject
{
    /// <summary>The invoice recipient first name.</summary>
    public string FirstName { get; set; }

    /// <summary>The invoice recipient last name.</summary>
    public string LastName { get; set; }

    /// <summary>The invoice recipient company business name.</summary>
    public string BusinessName { get; set; }

    /// <summary>The invoice recipient address.</summary>
    public InvoiceAddress Address { get; set; }
}