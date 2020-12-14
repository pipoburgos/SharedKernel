using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IDeleteRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        Task RemoveAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task RemoveRangeAsync(IEnumerable<TAggregate> aggregate, CancellationToken cancellationToken);
    }
}