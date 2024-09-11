namespace SharedKernel.Application.Validator;

/// <summary>An exception that represents failed validation</summary>
[Serializable]
public class ValidationFailureException : Exception
{
    /// <summary>Validation errors</summary>
    public IEnumerable<ValidationFailure> Errors { get; private set; }

    /// <summary>Creates a new ValidationException</summary>
    public ValidationFailureException(string message)
        : this(message, [])
    {
    }

    /// <summary>Creates a new ValidationException</summary>
    public ValidationFailureException(string message, IEnumerable<ValidationFailure> errors)
        : base(message)
    {
        Errors = errors;
    }

    /// <summary>Creates a new ValidationException</summary>
    // ReSharper disable once PossibleMultipleEnumeration
    public ValidationFailureException(IEnumerable<ValidationFailure> errors) : base(BuildErrorMessage(errors))
    {
        // ReSharper disable once PossibleMultipleEnumeration
        Errors = errors;
    }

    private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors)
    {
        var values = errors.Select(x => Environment.NewLine + " -- " + x.PropertyName + ": " + x.ErrorMessage);
        return $"Validation failed: {string.Join(string.Empty, values)}";
    }
}
