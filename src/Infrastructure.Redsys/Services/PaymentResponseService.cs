using Microsoft.Extensions.Options;
using SharedKernel.Infrastructure.Redsys.Contracts;

namespace SharedKernel.Infrastructure.Redsys.Services;

/// <summary> . </summary>
internal sealed class PaymentResponseService : IPaymentResponseService
{
    private readonly IMerchantParametersManager _merchantParamentersManager;
    private readonly ISignatureManager _signatureManager;
    private readonly ISignatureComparer _signatureComparer;
    private readonly IOptionsSnapshot<RedsysOptions> _options;

    /// <summary> . </summary>
    public PaymentResponseService(
        IMerchantParametersManager merchantParamentersManager,
        ISignatureManager signatureManager,
        ISignatureComparer signatureComparer,
        IOptionsSnapshot<RedsysOptions> options)
    {
        _merchantParamentersManager = merchantParamentersManager;
        _signatureManager = signatureManager;
        _signatureComparer = signatureComparer;
        _options = options;
    }

    /// <summary> . </summary>
    public ProcessedPayment GetProcessedPayment(string merchantParameters, string platformSignature)
    {
        var paymentResponse = _merchantParamentersManager.GetPaymentResponse(merchantParameters);
        var expectedSignature = _signatureManager.GetSignature(merchantParameters, paymentResponse.Ds_Order, _options.Value.Key);
        var isValidSignature = _signatureComparer.ValidateResponseSignature(expectedSignature, platformSignature);
        var result = new ProcessedPayment(paymentResponse, isValidSignature);
        return result;
    }

}