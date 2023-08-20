using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Validator;
using DataAnno = System.ComponentModel.DataAnnotations;
using IValidatableObject = SharedKernel.Domain.Validators.IValidatableObject;
using ValidationContext = SharedKernel.Domain.Validators.ValidationContext;
using ValidationResult = SharedKernel.Domain.Validators.ValidationResult;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts
{
    /// <summary>  </summary>
    public class ValidatableObjectService : IValidatableObjectService
    {
        /// <summary>  </summary>
        public virtual void Validate(DbContext context)
        {
            var changedEntities = context.ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Deleted);

            var errors = new List<DataAnno.ValidationResult>();
            foreach (var e in changedEntities)
            {
                var vc = new DataAnno.ValidationContext(e.Entity, null, null);
                DataAnno.Validator.TryValidateObject(e.Entity, vc, errors, true);
            }
        }

        /// <summary>  </summary>
        public virtual void ValidateDomainEntities(DbContext context)
        {
            var changedEntities = context.ChangeTracker
                .Entries<IValidatableObject>()
                .Where(e => e.State != EntityState.Deleted);

            var validateContext = new ValidationContext();
            var validationResult = new List<ValidationResult>();
            foreach (var validatedEntity in changedEntities)
            {
                validationResult.AddRange(validatedEntity.Entity.Validate(validateContext));
            }

            var errors = validationResult
                .Select(e => new ValidationFailure(string.Join(",", e.MemberNames), e.ErrorMessage))
                .ToList();

            if (errors.Any())
                throw new ValidationFailureException("Validation errors.", errors);
        }
    }
}