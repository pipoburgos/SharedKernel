using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    /// <summary>  </summary>
    public interface IValidatableObjectService
    {
        /// <summary>  </summary>
        void Validate(DbContext context);

        /// <summary>  </summary>
        void ValidateDomainEntities(DbContext context);
    }
}
