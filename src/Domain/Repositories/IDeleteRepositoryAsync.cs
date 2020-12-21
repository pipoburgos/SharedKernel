using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IDeleteRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveAsync(TAggregate aggregate, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<TAggregate> aggregate, CancellationToken cancellationToken);
    }
}