using SharedKernel.Domain.Repositories.Save;

namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Synchronous unit of work pattern. </summary>
public interface IUnitOfWork : ISaveRepository
{
}
