using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class CustomSemaphore : ISemaphore
    {
        public async Task BlockAsync(SemaphoreSlim semaphore, Func<Task> executeAsync, CancellationToken cancellationToken)
        {
            // https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            // Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore,
            // code execution will proceed, otherwise this thread waits here until the semaphore is released.
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                await executeAsync();
            }
            finally
            {
                // When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore
                // when we are ready, or else we will end up with a Semaphore that is forever locked.
                // This is why it is important to do the Release within a try...finally clause;
                // program execution may crash or take a different path, this way you are guaranteed execution
                semaphore.Release();
            }
        }
    }
}
