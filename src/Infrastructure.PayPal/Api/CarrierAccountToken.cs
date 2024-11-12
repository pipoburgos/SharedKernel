
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a carrier account that can be used to fund a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CarrierAccountToken //: PayPalSerializableObject
{
    /// <summary>ID of a previously saved carrier account resource.</summary>
    public string? CarrierAccountId { get; set; }

    /// <summary>
    /// The unique identifier of the payer used when saving this carrier account instrument.
    /// </summary>
    public string? ExternalCustomerId { get; set; }
}