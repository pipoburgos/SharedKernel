
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A REST API hyper schema resource that provides schema information for a HATEOAS link.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class HyperSchema : PayPalRelationalObject
{
    /// <summary>
    /// 
    /// </summary>
    public string FragmentResolution { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? ReadOnly { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ContentEncoding { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string PathStart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string MediaType { get; set; }
}