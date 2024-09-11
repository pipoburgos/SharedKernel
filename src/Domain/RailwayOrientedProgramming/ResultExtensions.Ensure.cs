namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary> . </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    /// <summary> . </summary>
    public static Result<T> EnsureAppendError<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (predicate(result.Value))
            return result;

        var errors = result.Errors.ToList();
        errors.Add(error);
        return Result.Failure<T>(errors);
    }
}
