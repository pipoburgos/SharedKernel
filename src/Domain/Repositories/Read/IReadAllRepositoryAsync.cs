namespace SharedKernel.Domain.Repositories.Read;

/// <summary>  </summary>
public interface IReadAllRepositoryAsync<TAggregate> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    Task<List<TAggregate>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> NotAnyAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<long> CountAsync(CancellationToken cancellationToken);
}
