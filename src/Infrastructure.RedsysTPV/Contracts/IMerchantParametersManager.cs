using SharedKernel.Infrastructure.Redsys.Models;

namespace SharedKernel.Infrastructure.Redsys.Contracts;

/// <summary> . </summary>
internal interface IMerchantParametersManager
{
    /// <summary> . </summary>
    string GetMerchantParameters(MerchantParameters paymentRequest);

    /// <summary> . </summary>
    PaymentResponse GetPaymentResponse(string merchantParameters);
}