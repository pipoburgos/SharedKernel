
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Details of an Error
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Error //: PayPalSerializableObject
{
    /// <summary>Human readable, unique name of the error.</summary>
    public string Name { get; set; }

    /// <summary>
    /// Reference ID of the purchase_unit associated with this error
    /// </summary>
    [Obsolete]
    public string PurchaseUnitReferenceId { get; set; }

    /// <summary>
    /// PayPal internal identifier used for correlation purposes.
    /// </summary>
    public string DebugId { get; set; }

    /// <summary>Message describing the error.</summary>
    public string Message { get; set; }

    /// <summary>PayPal internal error code.</summary>
    [Obsolete]
    public string Code { get; set; }

    /// <summary>Additional details of the error</summary>
    public List<ErrorDetails> Details { get; set; }

    /// <summary>
    /// URI for detailed information related to this error for the developer.
    /// </summary>
    public string InformationLink { get; set; }
}