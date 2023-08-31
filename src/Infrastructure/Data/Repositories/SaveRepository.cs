using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.Repositories;

/// <summary>  </summary>
public abstract class SaveRepository : ISaveRepository
{
    /// <summary>  </summary>
    protected readonly UnitOfWork UnitOfWork;

    /// <summary>  </summary>
    protected SaveRepository(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return UnitOfWork.SaveChangesResult();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return UnitOfWork.Rollback();
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return UnitOfWork.RollbackResult();
    }
}
