using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IRepositoryAsync<TAggregateRoot> : IRepository<TAggregateRoot>,
        ICreateRepositoryAsync<TAggregateRoot>,
        IReadRepositoryAsync<TAggregateRoot>,
        IUpdateRepositoryAsync<TAggregateRoot>,
        IDeleteRepositoryAsync<TAggregateRoot>,
        IReadSpecificationRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        Task<int> RollbackAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}