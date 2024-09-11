using FluentValidation;
using SharedKernel.Application.Validator;

namespace SharedKernel.Infrastructure.FluentValidation;

/// <inheritdoc />
/// <summary>
/// Validator based on Data Annotations.
/// This validator use IValidatableObject interface and
/// ValidationAttribute ( hierarchy of this) for
/// perform validation
/// </summary>
public class FluentValidator<T> : IClassValidator<T>
{
    private readonly IValidator<T>? _validator;

    /// <summary> . </summary>
    public FluentValidator(IValidator<T>? validator = default)
    {
        _validator = validator;
    }

    /// <summary> . </summary>
    public List<ValidationFailure> ValidateList(T item)
    {
        if (_validator == default)
            return new List<ValidationFailure>();

        if (EqualityComparer<T>.Default.Equals(item, default!))
            return new List<ValidationFailure>();

        var context = new ValidationContext<T>(item);
        return _validator
            .Validate(context)
            .Errors
            .Where(f => f != null)
            .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
            .ToList();
    }

    /// <summary> . </summary>
    public void Validate(T item)
    {
        if (_validator == default)
            return;

        if (EqualityComparer<T>.Default.Equals(item, default!))
            return;

        var context = new ValidationContext<T>(item);
        var failures = _validator
            .Validate(context)
            .Errors
            .Where(f => f != null)
            .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
            .ToList();

        if (failures.Any())
            throw new ValidationFailureException(failures);
    }

    /// <summary> . </summary>
    public async Task<List<ValidationFailure>> ValidateListAsync(T item, CancellationToken cancellationToken)
    {
        if (_validator == default)
            return new List<ValidationFailure>();

        if (item == null)
            return new List<ValidationFailure>();

        var context = new ValidationContext<T>(item);

        var errors = await _validator.ValidateAsync(context, cancellationToken);

        return errors
            .Errors
            .Where(f => f != null)
            .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage, f.AttemptedValue))
            .ToList();
    }

    /// <summary> . </summary>
    public async Task ValidateAsync(T item, CancellationToken cancellationToken)
    {
        if (_validator == default)
            return;

        if (item == null)
            return;

        var context = new ValidationContext<T>(item);

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
