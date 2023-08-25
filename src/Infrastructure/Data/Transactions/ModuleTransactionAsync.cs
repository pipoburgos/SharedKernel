using SharedKernel.Application.Transactions;
using System.Diagnostics;
using System.Transactions;

namespace SharedKernel.Infrastructure.Data.Transactions;

/// <summary>  </summary>
public class ModuleTransactionAsync : IModuleTransactionAsync
{
    private TransactionScope? _transaction;

    /// <summary>  </summary>
    public void Begin()
    {
        _transaction ??= new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }

    /// <summary>  </summary>
    public void End()
    {
        _transaction?.Complete();
        _transaction?.Dispose();
    }

    private void ReleaseUnmanagedResources()
    {
        _transaction?.Dispose();
        _transaction = default;
    }

    /// <summary>  </summary>
    /// <param name="disposing"></param>
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        Debug.WriteLine("Transaction released !!!");
    }

    /// <summary>  </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>  </summary>
    ~ModuleTransactionAsync()
    {
        Dispose(false);
    }
}
