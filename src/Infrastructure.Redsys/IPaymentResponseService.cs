namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public interface IPaymentResponseService
{
    /// <summary> . </summary>
    ProcessedPayment GetProcessedPayment(string merchantParameters, string platformSignature);
}