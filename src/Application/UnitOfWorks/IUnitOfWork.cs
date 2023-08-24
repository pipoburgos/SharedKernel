namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Synchronous unit of work pattern. </summary>
public interface IUnitOfWork
{
    /// <summary>  </summary>
    int Rollback();

    /// <summary>  </summary>
    int SaveChanges();
}
