using System.Collections.Generic;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class Result
{
    /// <summary>  </summary>
    public static readonly Unit Unit = Unit.Value;

    /// <summary>  </summary>
    public static Result<T> Create<T>(T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Success<T>(this T value) => Result<T>.Create(value);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(IEnumerable<string> errors) => new(errors);

    /// <summary>  </summary>
    public static Result<T> Failure<T>(string error) => new(new List<string> { error });

    /// <summary>  </summary>
    public static Result<Unit> Success() => Result<Unit>.Create(Unit);

    /// <summary>  </summary>
    public static Result<Unit> Failure(IEnumerable<string> errors) => new(errors);

    /// <summary>  </summary>
    public static Result<Unit> Failure(string error) => new(new List<string> { error });
}