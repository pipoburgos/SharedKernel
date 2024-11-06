
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Measurement to represent item dimensions like length, width, height and weight etc.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Measurement //: PayPalSerializableObject
{
    /// <summary>Value this measurement represents.</summary>
    public string Value { get; set; }

    /// <summary>Unit in which the value is represented.</summary>
    public string Unit { get; set; }
}