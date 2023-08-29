namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadRepositoryAsync<TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task<TAggregate?> GetByIdAsync(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<TAggregate?> GetDeleteByIdAsync(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> CountAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync(TId key, CancellationToken cancellationToken);
}