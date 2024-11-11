// ReSharper disable InconsistentNaming
namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public sealed class PaymentResponse
{
    /// <summary> . </summary>
    public string? Ds_Date { get; set; }

    /// <summary> . </summary>
    public string? Ds_Hour { get; set; }

    /// <summary> . </summary>
    public string? Ds_Amount { get; set; }

    /// <summary> . </summary>
    public string? Ds_Currency { get; set; }

    /// <summary> . </summary>
    public string Ds_Order { get; set; } = null!;

    /// <summary> . </summary>
    public string? Ds_MerchantCode { get; set; }

    /// <summary> . </summary>
    public string? Ds_Terminal { get; set; }

    /// <summary> . </summary>
    public string? Ds_Response { get; set; }

    /// <summary> . </summary>
    public string? Ds_MerchantData { get; set; }

    /// <summary> . </summary>
    public string? Ds_SecurePayment { get; set; }

    /// <summary> . </summary>
    public string? Ds_TransactionType { get; set; }

    /// <summary> . </summary>
    public string? Ds_Card_Country { get; set; }

    /// <summary> . </summary>
    public string? Ds_AuthorisationCode { get; set; }

    /// <summary> . </summary>
    public string? Ds_ConsumerLanguage { get; set; }

    /// <summary> . </summary>
    public string? Ds_Card_Type { get; set; }
}