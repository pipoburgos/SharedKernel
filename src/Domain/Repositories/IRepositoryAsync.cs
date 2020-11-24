using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IRepositoryAsync<TAggregateRoot> : IRepository<TAggregateRoot>,
        ICreateRepositoryAsync<TAggregateRoot>,
        IReadRepositoryAsync<TAggregateRoot>,
        IUpdateRepositoryAsync<TAggregateRoot>,
        IDeleteRepositoryAsync<TAggregateRoot>,
        IReadSpecificationRepositoryAsync<TAggregateRoot>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}