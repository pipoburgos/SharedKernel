using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts
{
    public class SharedKernelDbContext : DbContextBase
    {
        public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options,
            IValidatableObjectService validatableObjectService = null, IAuditableService auditable = null)
            : base(options, "skr", typeof(SharedKernelDbContext).Assembly, validatableObjectService, auditable)
        {
        }
    }
}
