namespace SharedKernel.Domain.Repositories.Delete;

/// <summary> . </summary>
public interface IDeleteRepositoryAsync<in TAggregate> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary> . </summary>
    Task RemoveAsync(TAggregate aggregateRoot, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task RemoveRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
}
