
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A JSON Patch object used for doing partial updates to resources.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Patch //: PayPalSerializableObject
{
    /// <summary>The operation to perform.</summary>
    public string Op { get; set; }

    /// <summary>
    /// A JSON pointer that references a location in the target document where the operation is performed. A `string` value.
    /// </summary>
    public string Path { get; set; }

    /// <summary>New value to apply based on the operation.</summary>
    public object Value { get; set; }

    /// <summary>
    /// A string containing a JSON Pointer value that references the location in the target document to move the value from.
    /// </summary>
    public string From { get; set; }
}