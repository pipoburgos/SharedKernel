using System.Collections.Generic;
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
        /// Dispatch commands requests that does not return anything
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task Dispatch(IEnumerable<ICommandRequest> commands, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch commands requests that return something
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TResponse[]> Dispatch<TResponse>(IEnumerable<ICommandRequest<TResponse>> commands, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch commands requests on a queue.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="queueName">Queue name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DispatchOnQueue(IEnumerable<ICommandRequest> commands, string queueName, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch a command request on a queue.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="queueName">Queue name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DispatchOnQueue(ICommandRequest command, string queueName, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatch a command request on a queue.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command"></param>
        /// <param name="queueName">Queue name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TResponse> DispatchOnQueue<TResponse>(ICommandRequest<TResponse> command, string queueName, CancellationToken cancellationToken);

        /// <summary>
        /// Queue a command request to background service
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task QueueInBackground(ICommandRequest command);
    }
}
