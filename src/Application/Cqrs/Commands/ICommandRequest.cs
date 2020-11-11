using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Commands
{
    public interface ICommandRequest : IRequest
    {
    }

    // ReSharper disable once UnusedTypeParameter
    public interface ICommandRequest<out TResponse> : ICommandRequest
    {
    }
}
