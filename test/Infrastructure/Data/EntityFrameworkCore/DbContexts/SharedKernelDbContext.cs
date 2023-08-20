using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts
{
    public interface ISharedKernelUnitOfWork : IUnitOfWorkAsync
    {

    }

    public class SharedKernelDbContext : DbContextBase, ISharedKernelUnitOfWork
    {
        public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options,
            IValidatableObjectService validatableObjectService = null, IAuditableService auditable = null)
            : base(options, "skr", typeof(SharedKernelDbContext).Assembly, validatableObjectService, auditable)
        {
        }
    }
}
