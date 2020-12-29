using SharedKernel.Application.Transactions;
using System;
using System.Diagnostics;
using System.Transactions;

namespace SharedKernel.Infrastructure.Data.Transactions
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleTransactionAsync : IModuleTransactionAsync
    {
        #region Members

        private TransactionScope _transaction;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Begin()
        {
            _transaction ??= new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        // ReSharper disable once UnusedParameter.Global
        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            Debug.WriteLine("Transaction released !!!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~ModuleTransactionAsync()
        {
            Dispose(false);
        }
    }
}
