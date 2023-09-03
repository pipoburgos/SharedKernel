using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql;

public class PostgreSqlSharedKernelDbContext : SharedKernelDbContext
{
    public PostgreSqlSharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options,
        IClassValidatorService? classValidatorService = default, IAuditableService? auditableService = default) : base(
        options, classValidatorService, auditableService)
    {
    }
}
