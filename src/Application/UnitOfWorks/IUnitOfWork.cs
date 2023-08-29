using SharedKernel.Domain.Repositories;

namespace SharedKernel.Application.UnitOfWorks;

/// <summary> Synchronous unit of work pattern. </summary>
public interface IUnitOfWork : IPersistRepository
{
}
