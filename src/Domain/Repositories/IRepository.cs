using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    internal interface IRepository<TAggregateRoot> :
        ICreateRepository<TAggregateRoot>,
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>,
        IDeleteRepository<TAggregateRoot>,
        IReadSpecificationRepository<TAggregateRoot>,
        IPersistRepository
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}