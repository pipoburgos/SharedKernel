namespace SharedKernel.Domain.Repositories.Read;

/// <summary> . </summary>
public interface IReadOneRepositoryAsync<TAggregate, in TId> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary> . </summary>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<bool> AnyAsync(TId id, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken);
}