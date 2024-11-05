using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;

namespace SharedKernel.Infrastructure.Validator;

/// <summary> . </summary>
public class ClassValidatorService : IClassValidatorService
{
    /// <summary> . </summary>
    public void ValidateDataAnnotations<T>(IEnumerable<T> classes) where T : class
    {
        var errors = GetDataAnnotationsValidationResult(classes);

        if (errors.Any())
            throw new ValidationFailureException("Validation errors.",
                errors.Select(e => new ValidationFailure(e.ErrorMessage, string.Join(", ", e.MemberNames))));
    }

    /// <summary> . </summary>
    public Result<Unit> ValidateDataAnnotationsResult<T>(IEnumerable<T> classes) where T : class
    {
        var errors = GetDataAnnotationsValidationResult(classes);
        return errors.Any()
            ? Result.Failure<Unit>(errors.Select(e =>
                Error.Create(e.ErrorMessage ?? string.Empty, string.Join(", ", e.MemberNames))))
            : Result.Success();
    }

    /// <summary> . </summary>
    public virtual void ValidateValidatableObjects<T>(IEnumerable<T> validatableObjects) where T : class, IValidatableObject
    {
        var errors = GetValidationFailures(validatableObjects);

        if (errors.Any())
            throw new ValidationFailureException("Validation errors.", errors);
    }

    /// <summary> . </summary>
    public Result<Unit> ValidateValidatableObjectsResult<T>(IEnumerable<T> validatableObjects) where T : class, IValidatableObject
    {
        var errors = GetValidationFailures(validatableObjects);
        return errors.Any()
            ? Result.Failure<Unit>(errors.Select(e => Error.Create(e.ErrorMessage, e.PropertyName)))
            : Result.Success();
    }

    private static List<global::System.ComponentModel.DataAnnotations.ValidationResult> GetDataAnnotationsValidationResult<T>(
        IEnumerable<T> classes) where T : class
    {
        var errors = new List<global::System.ComponentModel.DataAnnotations.ValidationResult>();
        foreach (var @class in classes)
        {
            var vc = new global::System.ComponentModel.DataAnnotations.ValidationContext(@class, null, null);
            global::System.ComponentModel.DataAnnotations.Validator.TryValidateObject(@class, vc, errors, true);
        }

        return errors;
    }

    private static List<ValidationFailure> GetValidationFailures<T>(IEnumerable<T> validatableObjects)
        where T : class, IValidatableObject
    {

        var validateContext = new ValidationContext();
        var validationResult = new List<ValidationResult>();
        foreach (var validatableObject in validatableObjects)
        {
            validationResult.AddRange(validatableObject.Validate(validateContext));
        }

        var errors = validationResult
            .Select(e => new ValidationFailure(string.Join(",", e.MemberNames), e.ErrorMessage))
            .ToList();

        return errors;
    }


}
