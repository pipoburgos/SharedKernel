
namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Collection of payment response related fields returned from a payment request
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class ProcessorResponse //: PayPalSerializableObject
{
    /// <summary>
    /// Paypal normalized response code, generated from the processor's specific response code
    /// </summary>
    public string ResponseCode { get; set; }

    /// <summary>
    /// Address Verification System response code. https://developer.paypal.com/webapps/developer/docs/classic/api/AVSResponseCodes/
    /// </summary>
    public string AvsCode { get; set; }

    /// <summary>
    /// CVV System response code. https://developer.paypal.com/webapps/developer/docs/classic/api/AVSResponseCodes/
    /// </summary>
    public string CvvCode { get; set; }

    /// <summary>
    /// Provides merchant advice on how to handle declines related to recurring payments
    /// </summary>
    public string AdviceCode { get; set; }

    /// <summary>
    /// Response back from the authorization. Provided by the processor
    /// </summary>
    public string EciSubmitted { get; set; }

    /// <summary>
    /// Visa Payer Authentication Service status. Will be return from processor
    /// </summary>
    public string Vpas { get; set; }
}