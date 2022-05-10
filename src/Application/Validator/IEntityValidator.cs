using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Validator
{
    /// <summary> The entity validator base contract. </summary>
    public interface IEntityValidator<in TEntity>
    {
        /// <summary>  </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        List<ValidationFailure> ValidateList(TEntity item);

        /// <summary>  </summary>
        /// <param name="item"></param>
        void Validate(TEntity item);

        /// <summary>  </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<ValidationFailure>> ValidateListAsync(TEntity item, CancellationToken cancellationToken);

        /// <summary>  </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task ValidateAsync(TEntity item, CancellationToken cancellationToken);
    }
}
