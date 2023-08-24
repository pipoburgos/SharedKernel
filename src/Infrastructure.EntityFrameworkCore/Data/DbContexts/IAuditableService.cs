namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditableService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        void Audit(DbContext dbContext);
    }
}
