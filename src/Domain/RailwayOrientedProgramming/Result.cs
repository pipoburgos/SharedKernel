namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class Result
{
    /// <summary>  </summary>
    public static readonly Unit Unit = Unit.Value;

    /// <summary>  </summary>
    public static Result<T?> Empty<T>() => Result<T>.Empty();

    /// <summary>  </summary>
    public static Result<T> Create<T>(T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Success<T>(this T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(IEnumerable<Error> errors) => Result<T>.Create(errors);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(Error error) => Result<T>.Create(new List<Error> { error });

    /// <summary>  </summary>
    public static Result<Unit> Success() => Result<Unit>.Create(Unit);

    /// <summary>  </summary>
    public static Result<Unit> Failure(IEnumerable<Error> errors) => Result<Unit>.Create(errors);

    /// <summary>  </summary>
    public static Result<Unit> Failure(Error error) => Result<Unit>.Create(new List<Error> { error });

    /// <summary>  </summary>
    public static Result<Unit> NotFound() => Result<Unit>.Create(Unit);
}