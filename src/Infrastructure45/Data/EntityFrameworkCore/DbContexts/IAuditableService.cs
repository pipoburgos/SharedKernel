using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    public interface IAuditableService
    {
        void Audit(DbContext dbContext);
    }
}
