using SharedKernel.Application.Transactions;
using System;
using System.Diagnostics;
using System.Transactions;

namespace SharedKernel.Infrastructure.Data.Transactions
{
    public class ModuleTransactionAsync : IModuleTransactionAsync
    {
        #region Members

        private TransactionScope _transaction;

        #endregion

        public void Begin()
        {
            if (_transaction == null)
                _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void End()
        {
            _transaction.Complete();
            _transaction?.Dispose();
        }

        private void ReleaseUnmanagedResources()
        {
            _transaction?.Dispose();
            _transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            Debug.WriteLine("Transacción liberada!!!");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ModuleTransactionAsync()
        {
            Dispose(false);
        }
    }
}
