namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IRepository<TAggregateRoot, in TId> :
    ICreateRepository<TAggregateRoot>,
    IReadRepository<TAggregateRoot, TId>,
    IUpdateRepository<TAggregateRoot>,
    IDeleteRepository<TAggregateRoot>,
    IReadSpecificationRepository<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot
{
}