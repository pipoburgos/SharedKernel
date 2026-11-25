namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public readonly record struct Result<T>
{
    /// <summary> . </summary>
    public readonly T? Value;

    /// <summary> . </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary> . </summary>
    public readonly IEnumerable<IResultMessage> Messages;

    /// <summary> . </summary>
    public override string ToString()
    {
        return string.Join(", ", Messages.Select(x => x.ToString()));
    }

    /// <summary> . </summary>
    public bool IsSuccess => !IsFailure;

    /// <summary> . </summary>
    public bool IsFailure =>
        EqualityComparer<T?>.Default.Equals(Value, default) &&
        Messages != null &&
        Messages.Any(x => x.IsError);

    /// <summary> . </summary>
    private Result(T value)
    {
        Value = value;
        Messages = Enumerable.Empty<IResultMessage>();
    }

    /// <summary> . </summary>
    private Result(IEnumerable<IResultMessage> messages)
    {
        if (messages == null)
            throw new InvalidOperationException("At least one error.");

        var list = messages.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("At least one error.");

        Value = default!;
        Messages = list;
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
    public static Result<T> Create(IEnumerable<IResultMessage> errors)
    {
        return new Result<T>(errors);
    }
}