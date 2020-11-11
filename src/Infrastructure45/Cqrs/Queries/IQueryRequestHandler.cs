using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Infrastructure.Cqrs.Queries
{
    public interface IQueryRequestHandler<in TRequest, TResponse> where TRequest : IQueryRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken);
    }
}
