
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A list of credit card resources returned from a search operation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CreditCardList : PayPalRelationalObject
{
    /// <summary>A list of credit card resources</summary>
    public List<CreditCard> Items { get; set; }

    /// <summary>
    /// Total number of items present in the given list. Note that the number of items might be larger than the records in the current page.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages that exist, for the total number of items, with the given page size.
    /// </summary>
    public int TotalPages { get; set; }
}