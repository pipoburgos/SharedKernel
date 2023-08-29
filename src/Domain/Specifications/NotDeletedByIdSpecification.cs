namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class NotDeletedByIdSpecification<T, TKey> : Specification<T>
    where T : class, IEntity<TKey>, IEntityAuditableLogicalRemove where TKey : notnull
{
    private TKey Key { get; }

    /// <summary>  </summary>
    public NotDeletedByIdSpecification(TKey key)
    {
        Key = key;
    }

    /// <summary>  </summary>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return new AndSpecification<T>(new NotDeletedSpecification<T>(), new EntityByIdSpecification<T, TKey>(Key))
            .SatisfiedBy();
    }
}
