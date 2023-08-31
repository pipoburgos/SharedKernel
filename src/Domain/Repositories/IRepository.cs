using SharedKernel.Domain.Repositories.Create;
using SharedKernel.Domain.Repositories.Delete;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Update;

namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IRepository<TAggregateRoot, in TId> :
    ICreateRepository<TAggregateRoot>,
    IReadOneRepository<TAggregateRoot, TId>,
    IUpdateRepository<TAggregateRoot>,
    IDeleteRepository<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot
{
}