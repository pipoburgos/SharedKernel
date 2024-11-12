
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Audit information for the resource.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Metadata //: PayPalSerializableObject
{
    /// <summary>The date and time when the resource was created.</summary>
    public string CreatedDate { get; set; }

    /// <summary>
    /// The email address of the account that created the resource.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>The date and time when the resource was canceled.</summary>
    public string CancelledDate { get; set; }

    /// <summary>The actor who canceled the resource.</summary>
    public string CancelledBy { get; set; }

    /// <summary>The date and time when the resource was last edited.</summary>
    public string LastUpdatedDate { get; set; }

    /// <summary>
    /// The email address of the account that last edited the resource.
    /// </summary>
    public string LastUpdatedBy { get; set; }

    /// <summary>The date and time when the resource was first sent.</summary>
    public string FirstSentDate { get; set; }

    /// <summary>The date and time when the resource was last sent.</summary>
    public string LastSentDate { get; set; }

    /// <summary>
    /// The email address of the account that last sent the resource.
    /// </summary>
    public string LastSentBy { get; set; }

    /// <summary>URL representing the payer's view of the invoice.</summary>
    public string PayerViewUrl { get; set; }
}