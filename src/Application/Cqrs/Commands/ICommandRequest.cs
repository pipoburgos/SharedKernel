using SharedKernel.Application.Requests;
using SharedKernel.Domain.Requests;

namespace SharedKernel.Application.Cqrs.Commands
{
    /// <summary>
    /// Command request that does not return anything
    /// </summary>
    public interface ICommandRequest : IRequest
    {
    }

    /// <summary>
    /// Command request that returns a response
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICommandRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
