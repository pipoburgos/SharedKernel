using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Commands
{
    public interface ICommandBus
    {
        Task<TResponse> Dispatch<TResponse>(ICommandRequest<TResponse> command, CancellationToken cancellationToken = default);

        Task Dispatch(ICommandRequest command, CancellationToken cancellationToken = default);

        Task DispatchInBackground(ICommandRequest command, CancellationToken cancellationToken = default);

        Task QueueInBackground(ICommandRequest command);
    }
}
