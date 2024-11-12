
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Invoice search parameters.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Search //: PayPalSerializableObject
{
    /// <summary>The initial letters of the email address.</summary>
    public string Email { get; set; }

    /// <summary>The initial letters of the recipient first name.</summary>
    public string RecipientFirstName { get; set; }

    /// <summary>The initial letters of the recipient last name.</summary>
    public string RecipientLastName { get; set; }

    /// <summary>The initial letters of the recipient business name.</summary>
    public string RecipientBusinessName { get; set; }

    /// <summary>The invoice number.</summary>
    public string Number { get; set; }

    /// <summary>The invoice status.</summary>
    public string Status { get; set; }

    /// <summary>The lower limit of the total amount.</summary>
    public PayPalCurrency LowerTotalAmount { get; set; }

    /// <summary>The upper limit of total amount.</summary>
    public PayPalCurrency UpperTotalAmount { get; set; }

    /// <summary>
    /// The start date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string StartInvoiceDate { get; set; }

    /// <summary>
    /// The end date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string EndInvoiceDate { get; set; }

    /// <summary>
    /// The start due date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string StartDueDate { get; set; }

    /// <summary>
    /// The end due date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string EndDueDate { get; set; }

    /// <summary>
    /// The start payment date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string StartPaymentDate { get; set; }

    /// <summary>
    /// The end payment date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string EndPaymentDate { get; set; }

    /// <summary>
    /// The start creation date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string StartCreationDate { get; set; }

    /// <summary>
    /// The end creation date for the invoice. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string EndCreationDate { get; set; }

    /// <summary>The offset for the search results.</summary>
    public float Page { get; set; }

    /// <summary>The page size for the search results.</summary>
    public float PageSize { get; set; }

    /// <summary>
    /// Indicates whether the total count appears in the response. Default is `false`.
    /// </summary>
    public bool? TotalCountRequired { get; set; }

    /// <summary>
    /// Indicates whether to list merchant-archived invoices in the response. If `true`, response lists only merchant-archived invoices. If `false`, response lists only unarchived invoices. If `null`, response lists all invoices.
    /// </summary>
    public bool? Archived { get; set; }
}