
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A HATEOAS (Hypermedia as the Engine of Application State) link included with most PayPal REST API resource objects.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Links //: PayPalSerializableObject
{
    /// <summary>
    /// 
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Rel { get; set; }
}