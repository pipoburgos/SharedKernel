namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IPersistRepository
{
    /// <summary>  </summary>
    int SaveChanges();

    /// <summary>  </summary>
    Result<int> SaveChangesResult();

    /// <summary>  </summary>
    int Rollback();
}
