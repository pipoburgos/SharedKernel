using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Domain.Repositories
{
    public interface IPersistRepositoryAsync
    {
        Task<int> RollbackAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
