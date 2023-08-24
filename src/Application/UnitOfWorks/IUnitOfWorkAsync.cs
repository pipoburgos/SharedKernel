namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Asynchronous unit of work pattern. </summary>
public interface IUnitOfWorkAsync : IUnitOfWork
{
    /// <summary>  </summary>
    Task<int> RollbackAsync(CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
