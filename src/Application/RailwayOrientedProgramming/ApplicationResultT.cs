using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public readonly struct ApplicationResult<T>
{
    /// <summary>  </summary>
    public readonly T Value;

    /// <summary>  </summary>
    public static implicit operator ApplicationResult<T>(T value) => new(value);

    /// <summary>  </summary>
    public readonly IEnumerable<ApplicationError> Errors;

    /// <summary>  </summary>
    public bool IsSuccess => !Errors.Any();

    /// <summary>  </summary>
    public bool IsFailure => Errors.Any();

    /// <summary>  </summary>
    private ApplicationResult(T value)
    {
        Value = value;
        Errors = Enumerable.Empty<ApplicationError>();
    }

    /// <summary>  </summary>
    private ApplicationResult(IEnumerable<ApplicationError> errors)
    {
        var list = errors.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("At least one error.");

        Value = default;
        Errors = list;
    }

    /// <summary>  </summary>
    public static ApplicationResult<T> Create(T value)
    {
        return new ApplicationResult<T>(value);
    }

    /// <summary>  </summary>
    public static ApplicationResult<T> Create(IEnumerable<ApplicationError> errors)
    {
        return new ApplicationResult<T>(errors);
    }
}