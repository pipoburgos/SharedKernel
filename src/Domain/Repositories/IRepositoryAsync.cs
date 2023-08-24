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
    /// <typeparam name="TKey"></typeparam>
    public interface IRepositoryAsync<TAggregateRoot, in TKey> : IRepository<TAggregateRoot, TKey>,
        ICreateRepositoryAsync<TAggregateRoot>,
        IReadRepositoryAsync<TAggregateRoot, TKey>,
        IUpdateRepositoryAsync<TAggregateRoot>,
        IDeleteRepositoryAsync<TAggregateRoot>,
        IReadSpecificationRepositoryAsync<TAggregateRoot>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}