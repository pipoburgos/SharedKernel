using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.System
{
    public interface ISemaphore
    {
        Task BlockAsync(SemaphoreSlim semaphoreSlim, Func<Task> executeAsync, CancellationToken cancellationToken);
    }
}
