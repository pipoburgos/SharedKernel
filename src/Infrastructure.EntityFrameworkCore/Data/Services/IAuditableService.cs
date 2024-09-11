namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;

/// <summary> . </summary>
public interface IAuditableService
{
    /// <summary> . </summary>
    /// <param name="dbContext"></param>
    void Audit(DbContext dbContext);
}
