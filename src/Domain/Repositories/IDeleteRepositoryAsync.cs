namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IDeleteRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task RemoveAsync(TAggregate aggregate, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task RemoveRangeAsync(IEnumerable<TAggregate> aggregate, CancellationToken cancellationToken);
}
