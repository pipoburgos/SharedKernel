
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Template settings.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceTemplateSettings //: PayPalSerializableObject
{
    /// <summary>
    /// The field name for any field in `template_data` for which to map corresponding display preferences.
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>The settings metadata for each field.</summary>
    public InvoiceTemplateSettingsMetadata DisplayPreference { get; set; }
}