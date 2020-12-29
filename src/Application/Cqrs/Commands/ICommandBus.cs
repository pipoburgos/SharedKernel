using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Commands
{
    /// <summary>
    /// Command bus
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Dispatch a command that returns a response
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TResponse> Dispatch<TResponse>(ICommandRequest<TResponse> command, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch a command request that does not return anything
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task Dispatch(ICommandRequest command, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch a command request in a background process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DispatchInBackground(ICommandRequest command, CancellationToken cancellationToken);

        /// <summary>
        /// Queue a command request to background service
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task QueueInBackground(ICommandRequest command);
    }
}
