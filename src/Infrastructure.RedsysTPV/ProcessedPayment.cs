namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public sealed class ProcessedPayment
{
    /// <summary> . </summary>
    public bool IsValidSignature { get; }

    /// <summary> . </summary>
    public bool IsPaymentPerformed { get; }

    /// <summary> . </summary>
    public PaymentResponse PaymentResponse { get; }

    /// <summary> . </summary>
    public ProcessedPayment(PaymentResponse paymentResponse, bool isValidSignature)
    {
        IsValidSignature = isValidSignature;
        IsPaymentPerformed = CheckPayment(paymentResponse, IsValidSignature);
        PaymentResponse = paymentResponse;
    }

    /// <summary> . </summary>
    private static bool CheckPayment(PaymentResponse paymentResponse, bool isValidSignature)
    {
        if (!isValidSignature) return false;

        return Convert.ToInt32(paymentResponse.Ds_Response) == 0;
    }
}