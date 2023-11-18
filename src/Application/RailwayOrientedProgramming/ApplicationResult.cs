namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class Result
{
    /// <summary>  </summary>
    public static readonly ApplicationUnit Unit = ApplicationUnit.Value;

    /// <summary>  </summary>
    public static Result<T> Create<T>(T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Success<T>(this T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(IEnumerable<ApplicationError> errors) => Result<T>.Create(errors);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(ApplicationError error) => Result<T>.Create(new List<ApplicationError> { error });

    /// <summary>  </summary>
    public static Result<ApplicationUnit> Success() => Result<ApplicationUnit>.Create(Unit);

    /// <summary>  </summary>
    public static Result<ApplicationUnit> Failure(IEnumerable<ApplicationError> errors) => Result<ApplicationUnit>.Create(errors);

    /// <summary>  </summary>
    public static Result<ApplicationUnit> Failure(ApplicationError error) => Result<ApplicationUnit>.Create(new List<ApplicationError> { error });
}