
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Details about a specific error.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ErrorDetails //: PayPalSerializableObject
{
    /// <summary>Name of the field that caused the error.</summary>
    public string Field { get; set; }

    /// <summary>Reason for the error.</summary>
    public string Issue { get; set; }

    /// <summary>
    /// Reference ID of the purchase_unit associated with this error
    /// </summary>
    [Obsolete]
    public string PurchaseUnitReferenceId { get; set; }

    /// <summary>PayPal internal error code.</summary>
    [Obsolete]
    public string Code { get; set; }
}