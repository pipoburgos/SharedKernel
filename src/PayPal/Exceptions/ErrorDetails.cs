
namespace PayPal.Exceptions;

/// <summary>
/// Details about a specific error.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ErrorDetails //: PayPalSerializableObject
{
    /// <summary>Name of the field that caused the error.</summary>
    public string? Field { get; set; }

    /// <summary>Reason for the error.</summary>
    public string? Issue { get; set; }
}