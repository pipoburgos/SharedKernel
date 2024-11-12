
namespace PayPal.V1.Payments;

/// <summary>
/// Extended Address object used as shipping address in a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ShippingAddress : Address
{
    /// <summary>Address ID assigned in PayPal system.</summary>
    public string? Id { get; set; }

    /// <summary>Name of the recipient at this address.</summary>
    public string? RecipientName { get; set; }

    /// <summary>Default shipping address of the Payer.</summary>
    public bool? DefaultAddress { get; set; }

    /// <summary>Shipping Address marked as preferred by Payer.</summary>
    public bool? PreferredAddress { get; set; }
}