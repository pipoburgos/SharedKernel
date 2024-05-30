using SharedKernel.Application.Validator;

namespace SharedKernel.Api;

/// <summary>  </summary>
public class ValidationError
{
    /// <summary>  </summary>
    public ValidationError(ValidationFailureException exception)
    {
        Errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(a => ToCamelCase(a.Key), b => b.Select(z => z.ErrorMessage).ToArray());
    }

    /// <summary>  </summary>
    public Dictionary<string, string[]> Errors { get; }

    /// <summary>  </summary>
    public static string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

    /// <summary>  </summary>
    public static string Title => "One or more validation errors occurred.";

    /// <summary>  </summary>
    public static int Status => 400;

    /// <summary>  </summary>
    public static string TraceId => Guid.NewGuid().ToString();

    /// <summary>  </summary>
    private string ToCamelCase(string str) =>
        string.IsNullOrEmpty(str) || str.Length < 2
            ? str
            : char.ToLowerInvariant(str[0]) + str[1..];
}