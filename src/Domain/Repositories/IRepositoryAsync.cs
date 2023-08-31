using SharedKernel.Domain.Repositories.Create;
using SharedKernel.Domain.Repositories.Delete;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Update;

namespace SharedKernel.Domain.Repositories;

/// <summary> An asynchronous generic repository pattern with all crud actions. </summary>
public interface IRepositoryAsync<TAggregateRoot, in TId> :
    IRepository<TAggregateRoot, TId>,
    ICreateRepositoryAsync<TAggregateRoot>,
    IReadOneRepositoryAsync<TAggregateRoot, TId>,
    IUpdateRepositoryAsync<TAggregateRoot>,
    IDeleteRepositoryAsync<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot
{
}