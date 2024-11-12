
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// The payment term of the invoice. If you specify `term_type`, you cannot specify `due_date`, and vice versa.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentTerm //: PayPalSerializableObject
{
    /// <summary>The terms by which the invoice payment is due.</summary>
    public string TermType { get; set; }

    /// <summary>
    /// The date when the invoice payment is due. This date must be a future date. Date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string DueDate { get; set; }
}