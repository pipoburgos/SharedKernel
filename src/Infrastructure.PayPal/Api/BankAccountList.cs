namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A list of bank accounts returned from a search operation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class BankAccountList : PayPalRelationalObject
{
    /// <summary>A list of bank account resources</summary>
    public List<BankAccount> Items { get; set; }

    /// <summary>
    /// Total number of items present in the given list. Note that the number of items might be larger than the records in the current page.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages that exist, for the total number of items, with the given page size.
    /// </summary>
    public int TotalPages { get; set; }
}