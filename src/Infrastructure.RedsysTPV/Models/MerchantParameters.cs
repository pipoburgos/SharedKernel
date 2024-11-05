using SharedKernel.Infrastructure.Redsys.Enums;

// ReSharper disable InconsistentNaming

namespace SharedKernel.Infrastructure.Redsys.Models;

/// <summary> . </summary>
internal sealed class MerchantParameters
{
    /// <summary> . </summary>
    public MerchantParameters(string Ds_Merchant_MerchantCode, string Ds_Merchant_Terminal,
        string Ds_Merchant_TransactionType, string Ds_Merchant_Amount, string Ds_Merchant_Currency,
        string Ds_Merchant_Order, string Ds_Merchant_MerchantURL, string Ds_Merchant_UrlOK,
        string Ds_Merchant_UrlKO, string Ds_Merchant_PayMethod, Language Ds_Merchant_ConsumerLanguage = Language.Spanish)
    {
        this.Ds_Merchant_MerchantCode = Ds_Merchant_MerchantCode;
        this.Ds_Merchant_Terminal = Ds_Merchant_Terminal;
        this.Ds_Merchant_TransactionType = Ds_Merchant_TransactionType;
        this.Ds_Merchant_Amount = Ds_Merchant_Amount;
        this.Ds_Merchant_Currency = Ds_Merchant_Currency;
        this.Ds_Merchant_Order = Ds_Merchant_Order;
        this.Ds_Merchant_MerchantURL = Ds_Merchant_MerchantURL;
        this.Ds_Merchant_UrlOK = Ds_Merchant_UrlOK;
        this.Ds_Merchant_UrlKO = Ds_Merchant_UrlKO;
        this.Ds_Merchant_ConsumerLanguage = $"{(int)Ds_Merchant_ConsumerLanguage:D3}";
        this.Ds_Merchant_PayMethod = Ds_Merchant_PayMethod;
    }

    /// <summary> . </summary>
    public string Ds_Merchant_ConsumerLanguage { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_Amount { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_Order { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_MerchantCode { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_Currency { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_TransactionType { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_Terminal { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_MerchantURL { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_UrlOK { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_UrlKO { get; }

    /// <summary> . </summary>
    public string Ds_Merchant_PayMethod { get; set; }
}