using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.UnitOfWorks
{
    /// <summary>
    /// Asynchronous unit of work pattern
    /// </summary>
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<int> RollbackAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}