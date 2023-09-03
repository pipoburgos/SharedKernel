using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Integration.Tests.Data;

public interface ISharedKernelUnitOfWork : IUnitOfWorkAsync { }

public class SharedKernelUnitOfWork : UnitOfWorkAsync, ISharedKernelUnitOfWork
{
    public SharedKernelUnitOfWork(IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
    }
}
