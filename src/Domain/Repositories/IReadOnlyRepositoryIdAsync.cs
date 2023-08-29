namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadOnlyRepositoryAsync<TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task<TAggregate?> GetByIdReadOnlyAsync(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<TAggregate?> GetDeleteByIdReadOnlyAsync(TId key, CancellationToken cancellationToken);
}