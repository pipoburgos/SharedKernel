using System.Collections.Generic;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class ApplicationResult
{
    /// <summary>  </summary>
    public static readonly ApplicationUnit Unit = ApplicationUnit.Value;

    /// <summary>  </summary>
    public static ApplicationResult<T> Create<T>(T value) => ApplicationResult<T>.Create(value);

    /// <summary>  </summary>
    public static ApplicationResult<T> Success<T>(this T value) => ApplicationResult<T>.Create(value);

    /// <summary>  </summary>
    public static ApplicationResult<T> Failure<T>(IEnumerable<ApplicationError> errors) => ApplicationResult<T>.Create(errors);

    /// <summary>  </summary>
    public static ApplicationResult<T> Failure<T>(ApplicationError error) => ApplicationResult<T>.Create(new List<ApplicationError> { error });

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Success() => ApplicationResult<ApplicationUnit>.Create(Unit);

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Failure(IEnumerable<ApplicationError> errors) => ApplicationResult<ApplicationUnit>.Create(errors);

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Failure(ApplicationError error) => ApplicationResult<ApplicationUnit>.Create(new List<ApplicationError> { error });
}