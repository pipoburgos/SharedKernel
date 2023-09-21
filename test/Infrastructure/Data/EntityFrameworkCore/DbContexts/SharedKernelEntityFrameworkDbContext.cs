using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

public class SharedKernelEntityFrameworkDbContext : EntityFrameworkDbContext, ISharedKernelEntityFrameworkUnitOfWork
{
    public SharedKernelEntityFrameworkDbContext(DbContextOptions<SharedKernelEntityFrameworkDbContext> options,
        IJsonSerializer? jsonSerializer = default, IClassValidatorService? classValidatorService = default,
        IAuditableService? auditableService = default) : base(options, "skr",
        typeof(SharedKernelEntityFrameworkDbContext).Assembly, jsonSerializer, classValidatorService, auditableService)
    {
    }
}
