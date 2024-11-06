namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Request object used for updating REST API resource objects.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PatchRequest : PayPalSerializableListObject<Patch>
{
}