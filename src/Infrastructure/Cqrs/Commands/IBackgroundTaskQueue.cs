using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        bool Any();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workItem"></param>
        void QueueBackground(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}