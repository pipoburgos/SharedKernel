using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

public class SharedKernelEntityFrameworkDbContext : EntityFrameworkDbContext, ISharedKernelEntityFrameworkDbContext
{
    public SharedKernelEntityFrameworkDbContext(DbContextOptions<SharedKernelEntityFrameworkDbContext> options,
        IClassValidatorService? classValidatorService = default, IAuditableService? auditableService = default) : base(
        options, "skr", typeof(SharedKernelEntityFrameworkDbContext).Assembly, classValidatorService, auditableService)
    {
    }
}
