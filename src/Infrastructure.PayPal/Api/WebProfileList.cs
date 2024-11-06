namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A list of web experience profiles resource objects returned from a search operation.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebProfileList : PayPalSerializableListObject<WebProfile>
{
}