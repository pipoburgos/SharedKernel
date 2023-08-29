using SharedKernel.Application.Validator;
using SharedKernel.Domain.RailwayOrientedProgramming;
using DataAnno = System.ComponentModel.DataAnnotations;
using IValidatableObject = SharedKernel.Domain.Validators.IValidatableObject;
using ValidationContext = SharedKernel.Domain.Validators.ValidationContext;
using ValidationResult = SharedKernel.Domain.Validators.ValidationResult;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

/// <summary>  </summary>
public class ValidatableObjectService : IValidatableObjectService
{
    /// <summary>  </summary>
    public virtual void Validate(DbContext context)
    {
        var errors = GetValidateErrors(context);

        if (errors.Any())
            throw new ValidationFailureException("Validation errors.",
                errors.Select(e => new ValidationFailure(e.ErrorMessage, string.Join(", ", e.MemberNames))));
    }

    /// <summary>  </summary>
    public Result<Unit> ValidateResul(DbContext context)
    {
        var errors = GetValidateErrors(context);
        return errors.Any()
            ? Result.Failure<Unit>(errors.Select(e =>
                Error.Create(e.ErrorMessage ?? string.Empty, string.Join(", ", e.MemberNames))))
            : Result.Success();
    }

    /// <summary>  </summary>
    public virtual void ValidateDomainEntities(DbContext context)
    {
        var errors = GetValidationFailures(context);

        if (errors.Any())
            throw new ValidationFailureException("Validation errors.", errors);
    }

    /// <summary>  </summary>
    public Result<Unit> ValidateDomainEntitiesResult(DbContext context)
    {
        var errors = GetValidationFailures(context);
        return errors.Any()
            ? Result.Failure<Unit>(errors.Select(e => Error.Create(e.ErrorMessage, e.PropertyName)))
            : Result.Success();
    }

    private List<DataAnno.ValidationResult> GetValidateErrors(DbContext context)
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

        return errors;
    }

    private List<ValidationFailure> GetValidationFailures(DbContext context)
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

        return errors;
    }
}
