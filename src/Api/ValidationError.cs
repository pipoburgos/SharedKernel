using SharedKernel.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Api;

internal class ValidationError
{
    public ValidationError(ValidationFailureException exception)
    {
        Errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(a => ToCamelCase(a.Key), b => b.Select(z => z.ErrorMessage).ToArray());
    }

    public Dictionary<string, string[]> Errors { get; }

    public static string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

    public static string Title => "One or more validation errors occurred.";

    public static int Status => 400;

    public static string TraceId => Guid.NewGuid().ToString();

    private string ToCamelCase(string str) =>
        string.IsNullOrEmpty(str) || str.Length < 2
            ? str
            : char.ToLowerInvariant(str[0]) + str[1..];
}