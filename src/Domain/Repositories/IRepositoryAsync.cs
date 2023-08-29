namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// An asynchronous generic repository pattern with all crud actions
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
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

    /// <summary>
    /// An asynchronous generic repository pattern with all crud actions
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepositoryAsync<TAggregateRoot, in TId> : IRepository<TAggregateRoot, TId>,
        ICreateRepositoryAsync<TAggregateRoot>,
        IReadRepositoryAsync<TAggregateRoot, TId>,
        IUpdateRepositoryAsync<TAggregateRoot>,
        IDeleteRepositoryAsync<TAggregateRoot>,
        IReadSpecificationRepositoryAsync<TAggregateRoot>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}