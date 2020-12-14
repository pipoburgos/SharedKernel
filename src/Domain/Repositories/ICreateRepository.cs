using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface ICreateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        void Add(TAggregate aggregate);

        void AddRange(IEnumerable<TAggregate> aggregates);
    }
}