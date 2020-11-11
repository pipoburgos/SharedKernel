using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.UnitOfWorks
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> RollbackAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}