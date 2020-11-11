using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Commands.Handlers
{
    public interface ICommandRequestHandler<in TCommand> where TCommand : ICommandRequest
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandRequestHandler<in TCommand, TResponse> where TCommand : ICommandRequest<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
