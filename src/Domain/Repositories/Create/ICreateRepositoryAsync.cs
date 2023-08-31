namespace SharedKernel.Domain.Repositories.Create;

/// <summary>  </summary>
public interface ICreateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task AddAsync(TAggregate aggregateRoot, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task AddRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
}
