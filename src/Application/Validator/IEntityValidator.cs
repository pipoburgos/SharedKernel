using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Validator
{
    /// <summary>
    /// The entity validator base contract
    /// </summary>
    public interface IEntityValidator<in TEntity>
    {
        void Validate(TEntity item);

        Task ValidateAsync(TEntity item, CancellationToken cancellationToken);
    }
}
