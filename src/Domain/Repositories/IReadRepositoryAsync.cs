using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IReadRepositoryAsync<TAggregate> where TAggregate : IAggregateRoot
    {
        Task<TAggregate> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken);

        Task<TAggregate> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken);

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<int> CountAsync(CancellationToken cancellationToken);

        Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken);
    }
}