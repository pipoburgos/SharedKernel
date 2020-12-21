using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public interface IRepository<TAggregateRoot> :
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