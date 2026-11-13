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
    public readonly string ErrorsText => $"Validation failed: {string.Join($"{Environment.NewLine} -- ", Errors.Select(x => x.ToString()))}";

    /// <summary> . </summary>
    public bool IsSuccess => Errors != default && !Errors.Any();

    /// <summary> . </summary>
    public bool IsFailure => Errors == default || Errors.Any();

    /// <summary> . </summary>
    private Result(T value)
    {
        Value = value;
        // ReSharper disable once UseCollectionExpression
        Errors = Enumerable.Empty<Error>();
        HttpStatusCode = HttpStatusCode.OK;
    }

    /// <summary> . </summary>
    private Result(IEnumerable<Error> errors)
    {
        if (errors == default)
            throw new InvalidOperationException("At least one error.");

        var list = errors.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("At least one error.");

        Value = default!;
        Errors = list;
        HttpStatusCode = HttpStatusCode.BadRequest;
    }

    /// <summary> . </summary>
    public static Result<T?> Empty()
    {
        return new Result<T?>((T?)default!);
    }

    /// <summary> . </summary>
    public static Result<T> Create(T value)
    {
        return new Result<T>(value);
    }

    /// <summary> . </summary>
    public static Result<T> Create(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>([Error.Create(error)]);
    }

    /// <summary> . </summary>
    public static Result<T> Create(Error error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>([error]);
    }

    /// <summary> . </summary>
    public static Result<T> Create(IEnumerable<Error> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(errors);
    }
}