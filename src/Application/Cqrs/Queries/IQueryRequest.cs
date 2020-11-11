using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Queries
{
    // ReSharper disable once UnusedTypeParameter
    public interface IQueryRequest<out TResponse> : IRequest
    {
    }
}
