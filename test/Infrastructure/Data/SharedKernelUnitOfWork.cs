using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Integration.Tests.Data;

public interface ISharedKernelUnitOfWork : IUnitOfWorkAsync { }

public class SharedKernelUnitOfWork : UnitOfWorkAsync, ISharedKernelUnitOfWork { }
