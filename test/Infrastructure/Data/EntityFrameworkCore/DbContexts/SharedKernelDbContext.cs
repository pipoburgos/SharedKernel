using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

public class SharedKernelDbContext : EntityFrameworkDbContext, ISharedKernelUnitOfWork
{
    public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options,
        IClassValidatorService? classValidatorService = default, IAuditableService? auditableService = default) : base(
        options, "skr", typeof(SharedKernelDbContext).Assembly, classValidatorService, auditableService)
    {
    }
}
