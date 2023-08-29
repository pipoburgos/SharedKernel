namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IUpdateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task UpdateAsync(TAggregate aggregateRoot, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task UpdateRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
}
