namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// The state of the associated billing agreement.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class AgreementStateDescriptor //: PayPalSerializableObject
{
    /// <summary>Reason for changing the state of the agreement.</summary>
    public string? Note { get; set; }

    /// <summary>The amount and currency of the agreement.</summary>
    public PayPalCurrency? Amount { get; set; }
}