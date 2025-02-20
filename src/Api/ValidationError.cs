using SharedKernel.Application.Validator;
using SharedKernel.Domain.Extensions;

namespace SharedKernel.Api;

/// <summary> . </summary>
public class ValidationError
{
    /// <summary> . </summary>
    public ValidationError(ValidationFailureException exception)
    {
        Errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(a => a.Key.ToCamelCase(), b => b.Select(z => z.ErrorMessage).ToArray());
    }

    /// <summary> . </summary>
    public Dictionary<string, string[]> Errors { get; }

    /// <summary> . </summary>
    public static string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

    /// <summary> . </summary>
    public static string Title => "One or more validation errors occurred.";

    /// <summary> . </summary>
    public static int Status => 400;

    /// <summary> . </summary>
    public static string TraceId => Guid.NewGuid().ToString();
}