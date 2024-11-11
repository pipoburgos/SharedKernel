using SharedKernel.Infrastructure.Redsys.Enums;

namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public sealed class PaymentRequest
{
    /// <summary> . </summary>
    /// <param name="merchantUrl"></param>
    /// <param name="urlOk"></param>
    /// <param name="urlKo"></param>
    /// <param name="order"></param>
    /// <param name="amount"></param>
    /// <param name="currency"> https://es.iban.com/currency-codes </param>
    /// <param name="payMethod"></param>
    /// <param name="language"></param>
    public PaymentRequest(Uri merchantUrl, Uri urlOk, Uri urlKo, string order, decimal amount,
        string currency = "978", PaymentMethod payMethod = PaymentMethod.CreditCard,
        Language language = Language.Spanish)
    {
        MerchantUrl = merchantUrl;
        UrlOk = urlOk;
        UrlKo = urlKo;
        Order = order;
        Amount = amount;
        Currency = currency;
        PayMethod = payMethod;
        Language = language;
    }

    /// <summary> . </summary>
    public Uri MerchantUrl { get; }

    /// <summary> . </summary>
    public Uri UrlOk { get; }

    /// <summary> . </summary>
    public Uri UrlKo { get; }

    /// <summary> . </summary>
    public string Order { get; }

    /// <summary> . </summary>
    public decimal Amount { get; }

    /// <summary> . </summary>
    public string Currency { get; }

    /// <summary> . </summary>
    public PaymentMethod PayMethod { get; }

    /// <summary> . </summary>
    public Language Language { get; }
}