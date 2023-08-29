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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TAggregateRoot, in TId> :
        ICreateRepository<TAggregateRoot>,
        IReadRepository<TAggregateRoot, TId>,
        IUpdateRepository<TAggregateRoot>,
        IDeleteRepository<TAggregateRoot>,
        IReadSpecificationRepository<TAggregateRoot>,
        IPersistRepository
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}