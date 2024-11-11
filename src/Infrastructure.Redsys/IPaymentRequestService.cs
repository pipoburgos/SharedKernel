namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public interface IPaymentRequestService
{
    /// <summary> . </summary>
    PaymentFormData GetPaymentRequestFormData(PaymentRequest paymentRequest);
}