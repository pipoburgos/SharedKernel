namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Synchronous unit of work pattern. </summary>
public interface IUnitOfWork
{
    /// <summary> . </summary>
    Guid Id { get; }

    /// <summary> . </summary>
    int SaveChanges();

    /// <summary> . </summary>
    Result<int> SaveChangesResult();

    /// <summary> . </summary>
    int Rollback();

    /// <summary> . </summary>
    Result<int> RollbackResult();
}
