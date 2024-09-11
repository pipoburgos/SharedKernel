namespace SharedKernel.Application.Transactions;

/// <summary> . </summary>
public interface IModuleTransactionAsync : IDisposable
{
    /// <summary> . </summary>
    void Begin();

    /// <summary> . </summary>
    void End();
}
