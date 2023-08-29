namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class EntityByIdSpecification<T, TKey> : Specification<T> where T : class, IEntity<TKey> where TKey : notnull
{
    private readonly TKey _key;

    /// <summary>  </summary>
    public EntityByIdSpecification(TKey key)
    {
        _key = key;
    }

    /// <summary>  </summary>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return e => e.Id.Equals(_key);
    }
}
