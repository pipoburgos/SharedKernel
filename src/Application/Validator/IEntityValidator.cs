using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Validator
{
    /// <summary>
    /// The entity validator base contract
    /// </summary>
    public interface IEntityValidator<in TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Validate(TEntity item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ValidateAsync(TEntity item, CancellationToken cancellationToken);
    }
}
