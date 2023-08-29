namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadOnlyRepositoryAsync<TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task<TAggregate?> GetByIdReadOnlyAsync<TId>(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    /// <returns></returns>
    Task<TAggregate?> GetDeleteByIdReadOnlyAsync<TId>(TId key, CancellationToken cancellationToken);
}
