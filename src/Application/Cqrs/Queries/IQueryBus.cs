using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Queries
{
    public interface IQueryBus
    {
        Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query);

        /// <summary>
        /// Envía una consulta para su ejecucción
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken);
    }
}
