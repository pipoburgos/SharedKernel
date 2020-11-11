using System;

namespace SharedKernel.Application.UnitOfWorks
{
    public interface IModuleTransactionAsync : IDisposable
    {
        void Begin();

        void End();
    }
}
