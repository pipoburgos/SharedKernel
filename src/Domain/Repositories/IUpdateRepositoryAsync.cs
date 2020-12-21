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
    public interface IUpdateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken);
    }
}