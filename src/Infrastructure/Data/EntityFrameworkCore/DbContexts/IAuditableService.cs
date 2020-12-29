using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
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
