using SharedKernel.Domain.Repositories.Save;

namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Asynchronous unit of work pattern. </summary>
public interface IUnitOfWorkAsync : IUnitOfWork, ISaveRepositoryAsync
{
}
