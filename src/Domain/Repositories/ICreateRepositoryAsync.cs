using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface ICreateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
    }
}