namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public readonly struct Result<T>
{
    /// <summary>  </summary>
    public readonly T Value;

    /// <summary>  </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>  </summary>
    public readonly IEnumerable<ApplicationError> Errors;

    /// <summary>  </summary>
    public bool IsSuccess => !Errors.Any();

    /// <summary>  </summary>
    public bool IsFailure => Errors.Any();

    /// <summary>  </summary>
    private Result(T value)
    {
        Value = value;
        Errors = Enumerable.Empty<ApplicationError>();
    }

    /// <summary>  </summary>
    private Result(IEnumerable<ApplicationError> errors)
    {
        var list = errors.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("At least one error.");

        Value = default!;
        Errors = list;
    }

    /// <summary>  </summary>
    public static Result<T> Create(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>  </summary>
    public static Result<T> Create(IEnumerable<ApplicationError> errors)
    {
        return new Result<T>(errors);
    }
}