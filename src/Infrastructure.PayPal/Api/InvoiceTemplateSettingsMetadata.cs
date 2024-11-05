
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Settings metadata for a template field.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceTemplateSettingsMetadata //: PayPalSerializableObject
{
    /// <summary>
    /// Indicates whether this field is hidden. Default is `false`.
    /// </summary>
    public bool? Hidden { get; set; }
}