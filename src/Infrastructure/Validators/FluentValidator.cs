using FluentValidation;
using SharedKernel.Application.Validator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Validators
{
    /// <inheritdoc />
    /// <summary>
    /// Validator based on Data Annotations.
    /// This validator use IValidatableObject interface and
    /// ValidationAttribute ( hierarchy of this) for
    /// perform validation
    /// </summary>
    public class FluentValidator<TEntity> : IEntityValidator<TEntity>
    {
        private readonly IValidator<TEntity> _validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        public FluentValidator(IValidator<TEntity> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public List<ValidationFailure> ValidateList(TEntity item)
        {
            if (item == null)
                return new List<ValidationFailure>();

            var context = new ValidationContext<TEntity>(item);
            return _validator
                .Validate(context)
                .Errors
                .Where(f => f != null)
                .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Validate(TEntity item)
        {
            if (item == null)
                return;

            var context = new ValidationContext<TEntity>(item);
            var failures = _validator
                .Validate(context)
                .Errors
                .Where(f => f != null)
                .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
                .ToList();

            if (failures.Any())
                throw new ValidationFailureException(failures);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task<List<ValidationFailure>> ValidateListAsync(TEntity item, CancellationToken cancellationToken)
        {
            if (item == null)
                return new List<ValidationFailure>();

            var context = new ValidationContext<TEntity>(item);

            var errors = await _validator.ValidateAsync(context, cancellationToken);

            return errors
                .Errors
                .Where(f => f != null)
                .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task ValidateAsync(TEntity item, CancellationToken cancellationToken)
        {
            if (item == null)
                return;

            var context = new ValidationContext<TEntity>(item);

            var failures = new List<ValidationFailure>();
            var result = await _validator.ValidateAsync(context, cancellationToken);

            if (!result.IsValid)
                failures.AddRange(result.Errors
                    .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
                    .ToList());

            if (failures.Any())
                throw new ValidationFailureException(failures);
        }
    }
}
