using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Commands
{
    public interface ICommandBus
    {
        Task<TResponse> Dispatch<TResponse>(ICommandRequest<TResponse> command, CancellationToken cancellationToken);

        Task Dispatch(ICommandRequest command, CancellationToken cancellationToken);

        Task DispatchInBackground(ICommandRequest command, CancellationToken cancellationToken);

        Task QueueInBackground(ICommandRequest command);
    }
}
