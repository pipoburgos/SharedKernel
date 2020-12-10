using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Queries
{
    public interface IQueryBus
    {
        /// <summary>
        /// Ask a query and return a data transfer object
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken);
    }
}
