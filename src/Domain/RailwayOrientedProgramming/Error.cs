namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public class Error
{
    /// <summary>  </summary>
    protected Error() { }

    /// <summary> Creates a new Error. </summary>
    protected Error(string errorMessage, string? propertyName = default)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }

    /// <summary> Creates a new Error. </summary>
    public static Error Create(string errorMessage, string? propertyName = default)
    {
        return new Error(errorMessage, propertyName);
    }

    /// <summary> The name of the property. </summary>
    public string? PropertyName { get; set; }

    /// <summary> The error message. </summary>
    public string ErrorMessage { get; set; } = null!;
}
