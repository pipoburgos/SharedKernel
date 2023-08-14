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
    public static ApplicationResult<T> Failure<T>(IEnumerable<string> errors) => ApplicationResult<T>.Create(errors);

    /// <summary>  </summary>
    public static ApplicationResult<T> Failure<T>(string error) => ApplicationResult<T>.Create(new List<string> { error });

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Success() => ApplicationResult<ApplicationUnit>.Create(Unit);

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Failure(IEnumerable<string> errors) => ApplicationResult<ApplicationUnit>.Create(errors);

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> Failure(string error) => ApplicationResult<ApplicationUnit>.Create(new List<string> { error });
}