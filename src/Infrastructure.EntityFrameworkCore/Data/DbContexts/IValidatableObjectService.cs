using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts
{
    /// <summary>  </summary>
    public interface IValidatableObjectService
    {
        /// <summary>  </summary>
        void Validate(DbContext context);

        /// <summary>  </summary>
        Result<Unit> ValidateResul(DbContext context);

        /// <summary>  </summary>
        void ValidateDomainEntities(DbContext context);

        /// <summary>  </summary>
        Result<Unit> ValidateDomainEntitiesResult(DbContext context);
    }
}
