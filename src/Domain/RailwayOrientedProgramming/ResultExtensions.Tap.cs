namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary> . </summary>
    public static Result<T> Tap<T>(this Result<T> r, Action<T> predicate)
    {
        if (r.IsSuccess)
            predicate(r.Value);

        return r;
    }
}
