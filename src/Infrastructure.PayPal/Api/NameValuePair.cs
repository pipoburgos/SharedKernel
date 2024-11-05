
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Used to define a type for name-value pairs.  The use of name value pairs in an API should be limited and approved by architecture.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class NameValuePair //: PayPalSerializableObject
{
    /// <summary>
    /// Key for the name value pair.  The value name types should be correlated
    /// </summary>
    public string Name { get; set; }

    /// <summary>Value for the name value pair.</summary>
    public string Value { get; set; }
}