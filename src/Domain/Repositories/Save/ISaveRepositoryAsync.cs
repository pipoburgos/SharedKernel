namespace SharedKernel.Domain.Repositories.Save;

/// <summary>  </summary>
public interface ISaveRepositoryAsync
{
    /// <summary>  </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> RollbackAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken);
}
