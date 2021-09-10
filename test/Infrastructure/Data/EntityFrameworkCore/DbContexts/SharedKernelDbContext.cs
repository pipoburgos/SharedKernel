using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts
{
    public class SharedKernelDbContext : DbContextBase
    {
        public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options, IAuditableService auditable = null)
            : base(options, "skr", typeof(SharedKernelDbContext).Assembly, auditable) { }
    }
}
