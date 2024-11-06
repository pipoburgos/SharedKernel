
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Payment terms for a billing plan.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Terms //: PayPalSerializableObject
{
    /// <summary>Identifier of the terms. 128 characters max.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Term type. Allowed values: `MONTHLY`, `WEEKLY`, `YEARLY`.
    /// </summary>
    public string Type { get; set; }

    /// <summary>Max Amount associated with this term.</summary>
    public PayPalCurrency MaxBillingAmount { get; set; }

    /// <summary>How many times money can be pulled during this term.</summary>
    public string Occurrences { get; set; }

    /// <summary>Amount_range associated with this term.</summary>
    public PayPalCurrency AmountRange { get; set; }

    /// <summary>Buyer's ability to edit the amount in this term.</summary>
    public string BuyerEditable { get; set; }
}