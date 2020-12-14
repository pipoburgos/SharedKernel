using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IDeleteRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        void Remove(TAggregate aggregate);

        void RemoveRange(IEnumerable<TAggregate> aggregate);
    }
}