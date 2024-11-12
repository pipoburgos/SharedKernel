
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a credit card that can be used to fund a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CreditCardToken //: PayPalSerializableObject
{
    /// <summary>
    /// ID of credit card previously stored using `/vault/credit-card`.
    /// </summary>
    public string? CreditCardId { get; set; }

    /// <summary>
    /// A unique identifier that you can assign and track when storing a credit card or using a stored credit card. This ID can help to avoid unintentional use or misuse of credit cards. This ID can be any value you would like to associate with the saved card, such as a UUID, username, or email address.  **Required when using a stored credit card if a payer_id was originally provided when storing the credit card in vault.**
    /// </summary>
    public string? PayerId { get; set; }

    /// <summary>Last four digits of the stored credit card number.</summary>
    public string? Last4 { get; set; }

    /// <summary>
    /// Credit card type. Valid types are: `visa`, `mastercard`, `discover`, `amex`. Values are presented in lowercase and not should not be used for display.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Expiration month with no leading zero. Acceptable values are 1 through 12.
    /// </summary>
    public int ExpireMonth { get; set; }

    /// <summary>4-digit expiration year.</summary>
    public int ExpireYear { get; set; }
}