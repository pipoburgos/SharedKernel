
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Merchant business information that appears on the invoice.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class MerchantInfo //: PayPalSerializableObject
{
    /// <summary>The merchant email address.</summary>
    public string Email { get; set; }

    /// <summary>The merchant first name.</summary>
    public string FirstName { get; set; }

    /// <summary>The merchant last name.</summary>
    public string LastName { get; set; }

    /// <summary>The merchant address.</summary>
    public InvoiceAddress Address { get; set; }

    /// <summary>The merchant company business name.</summary>
    public string BusinessName { get; set; }

    /// <summary>The merchant phone number.</summary>
    public Phone Phone { get; set; }

    /// <summary>The merchant fax number.</summary>
    public Phone Fax { get; set; }

    /// <summary>The merchant website.</summary>
    public string Website { get; set; }

    /// <summary>The merchant tax ID.</summary>
    public string TaxId { get; set; }

    /// <summary>A label for the `additional_info` field.</summary>
    public string AdditionalInfoLabel { get; set; }

    /// <summary>Additional information, such as business hours.</summary>
    public string AdditionalInfo { get; set; }
}