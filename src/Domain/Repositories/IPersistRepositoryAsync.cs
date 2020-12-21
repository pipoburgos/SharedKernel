using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPersistRepositoryAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> RollbackAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
