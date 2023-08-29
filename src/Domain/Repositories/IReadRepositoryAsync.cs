namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
/// <typeparam name="TAggregate"></typeparam>
public interface IReadRepositoryAsync<TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    /// <returns></returns>
    Task<TAggregate?> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    /// <returns></returns>
    Task<TAggregate?> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> CountAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync<TKey>(TKey key, CancellationToken cancellationToken);
}