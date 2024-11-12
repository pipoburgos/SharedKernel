
using PayPal.V1.Payments;

namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Base Address object used as billing address in a payment or extended for Shipping Address.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceAddress : BaseAddress
{
    /// <summary>Phone number in E.123 format.</summary>
    public Phone Phone { get; set; }
}