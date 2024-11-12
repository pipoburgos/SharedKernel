
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The custom amount to apply to an invoice. If you include a label, you must include a custom amount.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CustomAmount //: PayPalSerializableObject
{
    /// <summary>The custom amount label.</summary>
    public string? Label { get; set; }

    /// <summary>
    /// The custom amount value. Valid value is from -999999.99 to 999999.99.
    /// </summary>
    public PayPalCurrency? Amount { get; set; }
}