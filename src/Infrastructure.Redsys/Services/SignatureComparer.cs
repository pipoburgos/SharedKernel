using SharedKernel.Infrastructure.Redsys.Contracts;

namespace SharedKernel.Infrastructure.Redsys.Services;

/// <summary> . </summary>
internal sealed class SignatureComparer : ISignatureComparer
{
    /// <summary> . </summary>
    public bool ValidateResponseSignature(string expectedSignature, string providedSignature)
    {
        providedSignature = providedSignature
            .Replace("_", "/")
            .Replace("-", "+");

        var isValidSignature = providedSignature.Equals(expectedSignature);

        return isValidSignature;
    }
}