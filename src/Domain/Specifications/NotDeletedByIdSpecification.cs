namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class NotDeletedByIdSpecification<T, TId> : Specification<T>
    where T : class, IEntity<TId>, IEntityAuditableLogicalRemove where TId : notnull
{
    private TId Key { get; }

    /// <summary>  </summary>
    public NotDeletedByIdSpecification(TId key)
    {
        Key = key;
    }

    /// <summary>  </summary>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return new AndSpecification<T>(new NotDeletedSpecification<T>(), new EntityByIdSpecification<T, TId>(Key))
            .SatisfiedBy();
    }
}
