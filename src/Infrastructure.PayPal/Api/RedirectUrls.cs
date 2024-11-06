
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Set of redirect URLs you provide only for PayPal-based payments.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class RedirectUrls //: PayPalSerializableObject
{
    /// <summary>
    /// Url where the payer would be redirected to after approving the payment. **Required for PayPal account payments.**
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// Url where the payer would be redirected to after canceling the payment. **Required for PayPal account payments.**
    /// </summary>
    public string CancelUrl { get; set; }
}