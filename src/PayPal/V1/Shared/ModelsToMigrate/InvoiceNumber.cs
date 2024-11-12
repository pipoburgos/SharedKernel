
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceNumber //: PayPalSerializableObject
{
    /// <summary>
    /// The next invoice number that is available to the user. This number is auto-incremented from the most recent invoice number.
    /// </summary>
    public string Number { get; set; }
}