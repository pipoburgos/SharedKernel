using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Middlewares
{
    // ReSharper disable once UnusedTypeParameter
    public interface IRequest<TResponse> : IRequest
    {
    }
}
