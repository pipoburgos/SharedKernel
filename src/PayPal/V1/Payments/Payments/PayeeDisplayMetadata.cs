
using PayPal.V1.Shared;

namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Displays only metadata for a payee.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayeeDisplayMetadata : PayPalRelationalObject
{
    /// <summary>
    /// The email address for the payer. Maximum length is 127 characters.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>The PayPal-generated ID for the transaction.</summary>
    public DisplayPhone? DisplayPhone { get; set; }

    /// <summary>The payer's business name.</summary>
    public string? BrandName { get; set; }
}