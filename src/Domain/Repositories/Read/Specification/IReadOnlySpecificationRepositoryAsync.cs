namespace SharedKernel.Domain.Repositories.Read.Specification;

/// <summary> . </summary>
public interface IReadOnlySpecificationRepositoryAsync<TAggregateRoot> : IBaseRepository
    where TAggregateRoot : class, IAggregateRoot
{
    /// <summary> . </summary>
    Task<List<TAggregateRoot>> WhereReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<TAggregateRoot> SingleReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<TAggregateRoot?> SingleOrDefaultReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);
}
