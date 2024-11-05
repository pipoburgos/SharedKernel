namespace SharedKernel.Infrastructure.Redsys.Contracts;

/// <summary> . </summary>
internal interface ISignatureManager
{
    /// <summary> . </summary>
    string GetSignature(string merchantParameters, string merchantOrder, string merchantKey);
}