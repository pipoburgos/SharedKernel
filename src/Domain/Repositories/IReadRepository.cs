using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    internal interface IReadRepository<out TAggregate> where TAggregate : IAggregateRoot
    {
        TAggregate GetById<TKey>(TKey key);

        bool Any();

        bool Any<TKey>(TKey key);
    }
}