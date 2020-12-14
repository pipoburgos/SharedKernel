using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IUpdateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken);

        Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken);
    }
}