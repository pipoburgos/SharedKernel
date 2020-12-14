using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IUpdateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        void Update(TAggregate aggregate);

        void UpdateRange(IEnumerable<TAggregate> aggregates);
    }
}