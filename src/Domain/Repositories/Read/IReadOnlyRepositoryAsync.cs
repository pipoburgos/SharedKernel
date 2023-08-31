namespace SharedKernel.Domain.Repositories.Read;

/// <summary>  </summary>
public interface IReadOnlyRepositoryAsync<TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task<TAggregate?> GetByIdReadOnlyAsync(TId id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<TAggregate?> GetDeleteByIdReadOnlyAsync(TId id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<List<TAggregate>> GetAllReadOnlyAsync(CancellationToken cancellationToken);

}