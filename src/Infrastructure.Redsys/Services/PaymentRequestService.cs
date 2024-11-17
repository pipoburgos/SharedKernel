using Microsoft.Extensions.Options;
using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Enums;
using SharedKernel.Infrastructure.Redsys.Models;

namespace SharedKernel.Infrastructure.Redsys.Services;

/// <summary> . </summary>
internal sealed class PaymentRequestService : IPaymentRequestService
{
    private readonly IMerchantParametersManager _merchantParamentersManager;
    private readonly IOptionsSnapshot<RedsysOptions> _options;
    private readonly ISignatureManager _signatureManager;

    /// <summary> . </summary>
    public PaymentRequestService(
        ISignatureManager signatureManager,
        IMerchantParametersManager merchantParamentersManager,
        IOptionsSnapshot<RedsysOptions> options)
    {
        _merchantParamentersManager = merchantParamentersManager;
        _options = options;
        _signatureManager = signatureManager;
    }

    /// <summary> . </summary>
    public PaymentFormData GetPaymentRequestFormData(PaymentRequest paymentRequest)
    {
        var options = _options.Value;

        var payMethod = paymentRequest.PayMethod switch
        {
            PaymentMethod.CreditCard => "C",
            PaymentMethod.Transfer => "R",
            PaymentMethod.Bizum => "z",
            PaymentMethod.PayPal => "P",
            PaymentMethod.GooglePayApplePay => "xpay",
            _ => throw new ArgumentOutOfRangeException(nameof(paymentRequest.PayMethod), paymentRequest.PayMethod,
                null),
        };

        var merchantParameters = new MerchantParameters(options.MerchantCode, options.Terminal, options.TransactionType,
            GenerateAmount(paymentRequest.Amount), paymentRequest.Currency, paymentRequest.Order,
            paymentRequest.MerchantUrl.ToString(), paymentRequest.UrlOk.ToString(), paymentRequest.UrlKo.ToString(),
            payMethod, paymentRequest.Language);

        var merchantParametersString = _merchantParamentersManager.GetMerchantParameters(merchantParameters);

        var signature = _signatureManager.GetSignature(merchantParametersString, paymentRequest.Order, options.Key);

        return new PaymentFormData(options.FormUrl, options.SignatureVersion, merchantParametersString, signature);
    }

    private static string GenerateAmount(decimal price)
    {
        var t = price.ToString("#.##");
        var s = t.IndexOf(",", StringComparison.Ordinal) + 1;
        if (s == 0)
        {
            t += ".00";
        }
        else
        {
            var c = t.Split(',');
            if (c[1].Length < 2)
                t += "0";
        }
        var amount = t.Replace(",", string.Empty).Replace(".", string.Empty);
        return amount;
    }
}