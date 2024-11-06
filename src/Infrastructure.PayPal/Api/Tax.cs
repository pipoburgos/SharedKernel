
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Tax information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Tax //: PayPalSerializableObject
{
    /// <summary>The resource ID.</summary>
    public string Id { get; set; }

    /// <summary>The tax name.</summary>
    public string Name { get; set; }

    /// <summary>The tax rate. Valid value is from 0.001 to 99.999.</summary>
    public float Percent { get; set; }

    /// <summary>
    /// The tax, as a monetary amount. Cannot be specified in a request.
    /// </summary>
    public PayPalCurrency Amount { get; set; }
}