namespace SharedKernel.Infrastructure.Redsys.Contracts;

/// <summary> . </summary>
internal interface ISignatureComparer
{
    /// <summary> . </summary>
    bool ValidateResponseSignature(string expectedSignature, string providedSignature);
}