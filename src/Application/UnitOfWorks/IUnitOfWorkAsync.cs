using SharedKernel.Domain.Repositories;

namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Asynchronous unit of work pattern. </summary>
public interface IUnitOfWorkAsync : IUnitOfWork, IPersistRepositoryAsync
{
}
