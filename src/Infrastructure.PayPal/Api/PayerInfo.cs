
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a information about Payer.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayerInfo //: PayPalSerializableObject
{
    /// <summary>
    /// Email address representing the payer. 127 characters max.
    /// </summary>
    public string Email { get; set; }

    /// <summary>External Remember Me id representing the payer</summary>
    public string ExternalRememberMeId { get; set; }

    /// <summary>Account Number representing the Payer</summary>
    public string BuyerAccountNumber { get; set; }

    /// <summary>Salutation of the payer.</summary>
    public string Salutation { get; set; }

    /// <summary>First name of the payer.</summary>
    public string FirstName { get; set; }

    /// <summary>Middle name of the payer.</summary>
    public string MiddleName { get; set; }

    /// <summary>Last name of the payer.</summary>
    public string LastName { get; set; }

    /// <summary>Suffix of the payer.</summary>
    public string Suffix { get; set; }

    /// <summary>PayPal assigned encrypted Payer ID.</summary>
    public string PayerId { get; set; }

    /// <summary>
    /// Phone number representing the payer. 20 characters max.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>Phone type</summary>
    public string PhoneType { get; set; }

    /// <summary>
    /// Birth date of the Payer in ISO8601 format (yyyy-mm-dd).
    /// </summary>
    public string BirthDate { get; set; }

    /// <summary>
    /// Payer’s tax ID. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string TaxId { get; set; }

    /// <summary>
    /// Payer’s tax ID type. Allowed values: `BR_CPF` or `BR_CNPJ`. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string TaxIdType { get; set; }

    /// <summary>
    /// Two-letter registered country code of the payer to identify the buyer country.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>Billing address of the Payer.</summary>
    public Address BillingAddress { get; set; }

    /// <summary>
    /// [DEPRECATED] Use shipping address present in purchase unit or at root level of checkout Session.
    /// </summary>
    public ShippingAddress ShippingAddress { get; set; }
}