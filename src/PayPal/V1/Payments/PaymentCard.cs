using PayPal.V1.Shared;

namespace PayPal.V1.Payments;

/// <summary>
/// A payment card that can fund a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentCard : PayPalRelationalObject
{
    /// <summary>The ID of a credit card to save for later use.</summary>
    public string? Id { get; set; }

    /// <summary>The card number.</summary>
    public string? Number { get; set; }

    /// <summary>The card type.</summary>
    public string? Type { get; set; }

    /// <summary>The two-digit expiry month for the card.</summary>
    public int ExpireMonth { get; set; }

    /// <summary>The four-digit expiry year for the card.</summary>
    public int ExpireYear { get; set; }

    /// <summary>
    /// The two-digit start month for the card. Required for UK Maestro cards.
    /// </summary>
    public string? StartMonth { get; set; }

    /// <summary>
    /// The four-digit start year for the card. Required for UK Maestro cards.
    /// </summary>
    public string? StartYear { get; set; }

    /// <summary>
    /// The validation code for the card. Supported for payments but not for saving payment cards for future use.
    /// </summary>
    public string? Cvv2 { get; set; }

    /// <summary>The first name of the card holder.</summary>
    public string? FirstName { get; set; }

    /// <summary>The last name of the card holder.</summary>
    public string? LastName { get; set; }

    /// <summary>The two-letter country code.</summary>
    public string? BillingCountry { get; set; }

    /// <summary>The billing address for the card.</summary>
    public Address? BillingAddress { get; set; }

    /// <summary>
    /// The ID of the customer who owns this card account. The facilitator generates and provides this ID. Required when you create or use a stored funding instrument in the PayPal vault.
    /// </summary>
    public string? ExternalCustomerId { get; set; }

    /// <summary>The state of the funding instrument.</summary>
    public string? Status { get; set; }

    /// <summary>The product class of the financial instrument issuer.</summary>
    public string? CardProductClass { get; set; }

    /// <summary>
    /// The date and time until when this instrument can be used fund a payment.
    /// </summary>
    public string? ValidUntil { get; set; }

    /// <summary>
    /// The one- to two-digit card issue number. Required for UK Maestro cards.
    /// </summary>
    public string? IssueNumber { get; set; }
}