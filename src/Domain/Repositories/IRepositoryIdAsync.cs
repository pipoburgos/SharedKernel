namespace SharedKernel.Domain.Repositories;

/// <summary> An asynchronous generic repository pattern with all crud actions. </summary>
public interface IRepositoryAsync<TAggregateRoot, in TId> :
    IRepository<TAggregateRoot, TId>,
    ICreateRepositoryAsync<TAggregateRoot>,
    IReadRepositoryAsync<TAggregateRoot, TId>,
    IUpdateRepositoryAsync<TAggregateRoot>,
    IDeleteRepositoryAsync<TAggregateRoot>,
    IReadSpecificationRepositoryAsync<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot
{
}