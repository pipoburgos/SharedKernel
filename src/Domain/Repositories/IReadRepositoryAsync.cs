namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
/// <typeparam name="TAggregate"></typeparam>
public interface IReadRepositoryAsync<TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    /// <returns></returns>
    Task<TAggregate?> GetByIdAsync<TId>(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    /// <returns></returns>
    Task<TAggregate?> GetDeleteByIdAsync<TId>(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> CountAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync<TId>(TId key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync<TId>(TId key, CancellationToken cancellationToken);
}