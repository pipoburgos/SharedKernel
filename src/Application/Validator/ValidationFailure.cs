namespace SharedKernel.Application.Validator;

/// <summary>Defines a validation failure</summary>
[Serializable]
public class ValidationFailure
{
    private ValidationFailure() { }

    /// <summary>Creates a new ValidationFailure.</summary>
    public ValidationFailure(string? propertyName, string errorMessage, object? attemptedValue = default)
    {
        PropertyName = propertyName ?? string.Empty;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
    }

    /// <summary>The name of the property.</summary>
    public string PropertyName { get; set; } = null!;

    /// <summary>The error message</summary>
    public string ErrorMessage { get; set; } = null!;

    /// <summary>The property value that caused the failure.</summary>
    public object? AttemptedValue { get; set; }

    /// <summary>Creates a textual representation of the failure.</summary>
    public override string ToString()
    {
        return ErrorMessage;
    }
}
