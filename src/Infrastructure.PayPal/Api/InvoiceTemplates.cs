
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// List of merchant templates.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceTemplates : PayPalResource
{
    /// <summary>List of addresses in merchant profile.</summary>
    public List<Address> Addresses { get; set; }

    /// <summary>List of emails in merchant profile.</summary>
    public List<string> Emails { get; set; }

    /// <summary>List of phone numbers in merchant profile.</summary>
    public List<Phone> Phones { get; set; }

    /// <summary>An array of templates.</summary>
    public List<InvoiceTemplate> Templates { get; set; }
}