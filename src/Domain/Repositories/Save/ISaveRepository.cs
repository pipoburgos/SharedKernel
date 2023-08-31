namespace SharedKernel.Domain.Repositories.Save;

/// <summary>  </summary>
public interface ISaveRepository
{
    /// <summary>  </summary>
    int SaveChanges();

    /// <summary>  </summary>
    Result<int> SaveChangesResult();

    /// <summary>  </summary>
    int Rollback();

    /// <summary>  </summary>
    Result<int> RollbackResult();
}
