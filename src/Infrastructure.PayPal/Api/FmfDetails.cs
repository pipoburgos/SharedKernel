
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Details of Fraud Management Filter (FMF).
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FmfDetails //: PayPalSerializableObject
{
    /// <summary>Type of filter.</summary>
    public string FilterType { get; set; }

    /// <summary>Filter Identifier.</summary>
    public string FilterId { get; set; }

    /// <summary>Name of the filter</summary>
    public string Name { get; set; }

    /// <summary>Description of the filter.</summary>
    public string Description { get; set; }
}