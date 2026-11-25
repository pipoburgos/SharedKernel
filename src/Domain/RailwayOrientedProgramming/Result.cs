namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public static class Result
{
    /// <summary> . </summary>
    public static readonly Unit Unit = Unit.Value;

    /// <summary> . </summary>
    public static Result<T?> Empty<T>() => Result<T>.Empty();

    /// <summary> . </summary>
    public static Result<T> Create<T>(T value) => Result<T>.Create(value);

    /// <summary> . </summary>
    public static Result<T> Success<T>(this T value) => Result<T>.Create(value);

    /// <summary> . </summary>
    public static Result<T> Failure<T>(IEnumerable<IResultMessage> errors) => Result<T>.Create(errors);

    /// <summary> . </summary>
    public static Result<T> Failure<T>(IResultMessage error) => Result<T>.Create(new List<IResultMessage> { error });



    /// <summary> . </summary>
    public static Result<T> Created<T>(this T value) => Result<T>.Create(value);

    /// <summary> . </summary>
    public static Result<T> Ok<T>(this T value) => Result<T>.Create(value);

    /// <summary> . </summary>
    public static Result<Unit> Success() => Result<Unit>.Create(Unit);

    /// <summary> . </summary>
    public static Result<Unit> Ok() => Result<Unit>.Create(Unit);

    /// <summary> . </summary>
    public static Result<Unit> Failure(IEnumerable<IResultMessage> errors) => Result<Unit>.Create(errors);

    /// <summary> . </summary>
    public static Result<Unit> Failure(Error error) => Result<Unit>.Create([error]);
}