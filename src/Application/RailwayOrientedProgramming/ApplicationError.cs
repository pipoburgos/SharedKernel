namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public class ApplicationError
{
    /// <summary>  </summary>
    protected ApplicationError()
    {
        ErrorMessage = default!;
    }

    /// <summary> Creates a new Error. </summary>
    protected ApplicationError(string errorMessage, string? propertyName = default)
    {
        PropertyName = string.IsNullOrWhiteSpace(propertyName) ? string.Empty : propertyName;
        ErrorMessage = errorMessage;
    }

    /// <summary> Creates a new Error. </summary>
    public static ApplicationError Create(string errorMessage, string? propertyName = default)
    {
        return new ApplicationError(errorMessage, propertyName);
    }

    /// <summary> The name of the property. </summary>
    public string? PropertyName { get; set; }

    /// <summary> The error message. </summary>
    public string ErrorMessage { get; set; }
}
