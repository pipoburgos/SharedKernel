
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Parameters for flow configuration.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class FlowConfig //: PayPalSerializableObject
{
    /// <summary>
    /// The type of landing page to display on the PayPal site for user checkout. Set to `Billing` to use the non-PayPal account landing page. Set to `Login` to use the PayPal account login landing page.
    /// </summary>
    public string LandingPageType { get; set; }

    /// <summary>
    /// The merchant site URL to display after a bank transfer payment. Valid for only the Giropay or bank transfer payment method in Germany.
    /// </summary>
    public string BankTxnPendingUrl { get; set; }

    /// <summary>
    /// Defines whether buyers can complete purchases on the PayPal or merchant website.
    /// </summary>
    public string UserAction { get; set; }

    /// <summary>
    /// The HTTP method to use to redirect the user to a return URL. Valid value is `GET` or `POST`.
    /// </summary>
    public string ReturnUriHttpMethod { get; set; }
}