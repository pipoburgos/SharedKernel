
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Parameters for style and presentation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Presentation //: PayPalSerializableObject
{
    /// <summary>
    /// A label that overrides the business name in the PayPal account on the PayPal pages. Character length and limitations: 127 single-byte alphanumeric characters.
    /// </summary>
    public string BrandName { get; set; }

    /// <summary>
    /// A URL to the logo image. A valid media type is `.gif`, `.jpg`, or `.png`. The maximum width of the image is 190 pixels. The maximum height of the image is 60 pixels. PayPal crops images that are larger. PayPal places your logo image at the top of the cart review area. PayPal recommends that you store the image on a secure (HTTPS) server. Otherwise, web browsers display a message that checkout pages contain non-secure items. Character length and limit: 127 single-byte alphanumeric characters.
    /// </summary>
    public string LogoImage { get; set; }

    /// <summary>
    /// The locale of pages displayed by PayPal payment experience. A valid value is `AU`, `AT`, `BE`, `BR`, `CA`, `CH`, `CN`, `DE`, `ES`, `GB`, `FR`, `IT`, `NL`, `PL`, `PT`, `RU`, or `US`. A 5-character code is also valid for languages in specific countries: `da_DK`, `he_IL`, `id_ID`, `ja_JP`, `no_NO`, `pt_BR`, `ru_RU`, `sv_SE`, `th_TH`, `zh_CN`, `zh_HK`, or `zh_TW`.
    /// </summary>
    public string LocaleCode { get; set; }

    /// <summary>
    /// A label to use as hypertext for the return to merchant link.
    /// </summary>
    public string ReturnUrlLabel { get; set; }

    /// <summary>
    /// A label to use as the title for the note to seller field. Used only when `allow_note` is `1`.
    /// </summary>
    public string NoteToSellerLabel { get; set; }
}