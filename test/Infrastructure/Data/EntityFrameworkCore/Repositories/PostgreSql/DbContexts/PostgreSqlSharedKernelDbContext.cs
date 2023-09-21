using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts;

public class PostgreSqlSharedKernelDbContext : EntityFrameworkDbContext, IPostgreSqlSharedKernelUnitOfWork
{
    public PostgreSqlSharedKernelDbContext(DbContextOptions<PostgreSqlSharedKernelDbContext> options,
        IJsonSerializer? jsonSerializer = default, IClassValidatorService? classValidatorService = default,
        IAuditableService? auditableService = default) : base(options, "skr",
        typeof(SharedKernelEntityFrameworkDbContext).Assembly, jsonSerializer, classValidatorService, auditableService)
    {
    }
}
