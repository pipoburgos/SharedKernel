using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using SharedKernel.Application.Validator;

namespace SharedKernel.Infrastructure.Validators
{
    /// <inheritdoc />
    /// <summary>
    /// Validator based on Data Annotations.
    /// This validator use IValidatableObject interface and
    /// ValidationAttribute ( hierarchy of this) for
    /// perform validation
    /// </summary>
    public class FluentValidator<TRequest> : IEntityValidator<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validators"></param>
        public FluentValidator(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Validate(TRequest item)
        {
            if (item == null)
                return;

            var context = new ValidationContext<TRequest>(item);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
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
        public async Task ValidateAsync(TRequest item, CancellationToken cancellationToken)
        {
            if (item == null)
                return;

            var context = new ValidationContext<TRequest>(item);

            var failures = new List<ValidationFailure>();
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);

                if(!result.IsValid)
                    failures.AddRange(result.Errors
                        .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
                        .ToList());
            }

            if (failures.Any())
                throw new ValidationFailureException(failures);
        }
    }
}
