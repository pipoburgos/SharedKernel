using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    public interface IRepository<TAggregateRoot> :
        ICreateRepository<TAggregateRoot>,
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>,
        IDeleteRepository<TAggregateRoot>,
        IReadSpecificationRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        int Rollback();

        int Save();
    }
}