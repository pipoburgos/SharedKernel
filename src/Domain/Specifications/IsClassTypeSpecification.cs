namespace SharedKernel.Domain.Specifications;

/// <summary> . </summary>
public class IsClassTypeSpecification<T> : ISpecification<T>
{
    /// <summary> . </summary>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        return x =>
            typeof(T).IsClass &&
            typeof(T) != typeof(string) &&
            !typeof(IEnumerable).IsAssignableFrom(typeof(T));
    }
}
