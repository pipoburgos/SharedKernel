
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Parameters for input fields customization.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InputFields //: PayPalSerializableObject
{
    /// <summary>
    /// Indicates whether the buyer can enter a note to the merchant on the PayPal page during checkout.
    /// </summary>
    public bool? AllowNote { get; set; }

    /// <summary>
    /// Indicates whether PayPal displays shipping address fields on the experience pages. For digital goods, this field is required and must be <code>1</code>. Value is:<ul><li><code>0</code>. Displays the shipping address on the PayPal pages.</li><li><code>1</code>. Redacts shipping address fields from the PayPal pages.</li><li><code>2</code>. Gets the shipping address from the buyer's account profile.</li></ul>
    /// </summary>
    public int NoShipping { get; set; }

    /// <summary>
    /// Indicates whether to display the shipping address that is passed to this call rather than the one on file with PayPal for this buyer on the PayPal experience pages. Value is:<ul><li><code>0</code>. Displays the shipping address on file.</li><li><code>1</code>. Displays the shipping address supplied to this call. The buyer cannot edit this shipping address.</li></ul>
    /// </summary>
    public int AddressOverride { get; set; }
}