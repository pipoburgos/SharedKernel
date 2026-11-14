using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public readonly record struct Result<T>
{
    /// <summary> . </summary>
    public readonly HttpStatusCode HttpStatusCode;

    /// <summary> . </summary>
    public readonly T Value;

    /// <summary> . </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary> . </summary>
    public readonly IEnumerable<Error> Errors;

    /// <summary> . </summary>
    public readonly string ErrorsText => string.Join(", ", Errors.Select(x => x.ToString()));

    /// <summary> . </summary>
    public bool IsSuccess =>
        Errors != null && !Errors.Any() &&
        HttpStatusCode >= HttpStatusCode.OK && HttpStatusCode < HttpStatusCode.Ambiguous;

    /// <summary> . </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary> . </summary>
    private Result(T value, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        Value = value;
        // ReSharper disable once UseCollectionExpression
        Errors = Enumerable.Empty<Error>();
        if (HttpStatusCode < HttpStatusCode.OK || HttpStatusCode >= HttpStatusCode.Ambiguous)
            throw new InvalidOperationException("Invalid HTTP status code.");

        HttpStatusCode = httpStatusCode;
    }

    /// <summary> . </summary>
    private Result(IEnumerable<Error> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        if (errors == null)
            throw new InvalidOperationException("At least one error.");

        var list = errors.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("At least one error.");

        Value = default!;
        Errors = list;
        if (HttpStatusCode >= HttpStatusCode.OK && HttpStatusCode < HttpStatusCode.Ambiguous)
            throw new InvalidOperationException("Invalid HTTP status code.");

        HttpStatusCode = httpStatusCode;
    }

    /// <summary> . </summary>
    public static Result<T?> Empty()
    {
        return new Result<T?>((T?)default!);
    }

    /// <summary> . </summary>
    public static Result<T> Create(T value, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        return new Result<T>(value, httpStatusCode);
    }

    /// <summary> . </summary>
    public static Result<T> Create(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>([Error.Create(error)], httpStatusCode);
    }

    /// <summary> . </summary>
    public static Result<T> Create(Error error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>([error], httpStatusCode);
    }

    /// <summary> . </summary>
    public static Result<T> Create(IEnumerable<Error> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(errors, httpStatusCode);
    }
}