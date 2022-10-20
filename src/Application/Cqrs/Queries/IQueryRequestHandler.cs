using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Queries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQueryRequestHandler<in TRequest, TResponse> where TRequest : IQueryRequest<TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken);
    }
}
