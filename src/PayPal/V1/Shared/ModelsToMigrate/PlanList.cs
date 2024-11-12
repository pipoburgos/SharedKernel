
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// A list of billing plans returned from a search operation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PlanList : PayPalRelationalObject
{
    /// <summary>Array of billing plans.</summary>
    public List<Plan> Plans { get; set; }

    /// <summary>Total number of items.</summary>
    public string TotalItems { get; set; }

    /// <summary>Total number of pages.</summary>
    public string TotalPages { get; set; }
}