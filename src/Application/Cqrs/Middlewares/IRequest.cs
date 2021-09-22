using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Middlewares
{
    /// <summary>
    /// Request made to a bus
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IRequest<out TResponse> : IRequest
    {
    }
}
