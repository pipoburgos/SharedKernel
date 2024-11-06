
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The file attached to an invoice or template.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FileAttachment //: PayPalSerializableObject
{
    /// <summary>The name of the attached file.</summary>
    public string Name { get; set; }

    /// <summary>
    /// The URL of the attached file, which can be downloaded.
    /// </summary>
    public string Url { get; set; }
}