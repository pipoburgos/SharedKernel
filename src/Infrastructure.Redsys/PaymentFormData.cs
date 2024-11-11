// ReSharper disable InconsistentNaming
namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public sealed class PaymentFormData
{
    /// <summary> . </summary>
    public PaymentFormData(string formUrl, string dsSignatureVersion, string dsMerchantParameters, string dsSignature)
    {
        FormUrl = formUrl;
        Ds_SignatureVersion = dsSignatureVersion;
        Ds_MerchantParameters = dsMerchantParameters;
        Ds_Signature = dsSignature;
    }

    /// <summary> . </summary>
    public string FormUrl { get; }

    /// <summary> . </summary>
    public string Ds_SignatureVersion { get; }

    /// <summary> . </summary>
    public string Ds_MerchantParameters { get; }

    /// <summary> . </summary>
    public string Ds_Signature { get; }
}