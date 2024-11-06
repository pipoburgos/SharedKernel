
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a information about a potential Payer.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PotentialPayerInfo //: PayPalSerializableObject
{
    /// <summary>Email address representing the potential payer.</summary>
    public string Email { get; set; }

    /// <summary>
    /// xternalRememberMe id representing the potential payer.
    /// </summary>
    public string ExternalRememberMeId { get; set; }

    /// <summary>Billing address of the potential payer.</summary>
    public Address BillingAddress { get; set; }
}