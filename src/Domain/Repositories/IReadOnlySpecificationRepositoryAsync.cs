namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadOnlySpecificationRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
{
    /// <summary>  </summary>
    Task<List<TAggregateRoot>> WhereReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<TAggregateRoot> SingleReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<TAggregateRoot?> SingleOrDefaultReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);
}
