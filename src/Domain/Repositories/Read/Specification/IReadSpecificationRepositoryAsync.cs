namespace SharedKernel.Domain.Repositories.Read.Specification;

/// <summary> . </summary>
public interface IReadSpecificationRepositoryAsync<TAggregateRoot> : IBaseRepository
    where TAggregateRoot : class, IAggregateRoot
{
    /// <summary> . </summary>
    Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<TAggregateRoot?> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);
}
