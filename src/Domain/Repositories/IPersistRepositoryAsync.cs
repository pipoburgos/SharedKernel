namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IPersistRepositoryAsync
{
    /// <summary>  </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> RollbackAsync(CancellationToken cancellationToken);
}
