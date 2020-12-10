using System;

namespace SharedKernel.Application.Transactions
{
    public interface IModuleTransactionAsync : IDisposable
    {
        void Begin();

        void End();
    }
}
