namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts
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
