using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Models;

namespace SharedKernel.Infrastructure.Redsys.Services;

/// <summary> . </summary>
internal sealed class MerchantParametersManager : IMerchantParametersManager
{
    private readonly IBase64 _base64;
    private readonly IJsonSerializer _jsonSerializer;

    /// <summary> . </summary>
    public MerchantParametersManager(IBase64 base64, IJsonSerializer jsonSerializer)
    {
        _base64 = base64;
        _jsonSerializer = jsonSerializer;
    }

    /// <summary> . </summary>
    public string GetMerchantParameters(MerchantParameters paymentRequest)
    {
        var json = _jsonSerializer.Serialize(paymentRequest, NamingConvention.NoAction);
        return _base64.EncodeTo64(json);
    }

    /// <summary> . </summary>
    public PaymentResponse GetPaymentResponse(string merchantParameters)
    {
        var json = _base64.DecodeFrom64(merchantParameters);
        return _jsonSerializer.Deserialize<PaymentResponse>(json, NamingConvention.NoAction);
    }
}