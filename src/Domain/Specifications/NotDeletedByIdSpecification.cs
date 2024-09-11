namespace SharedKernel.Domain.Specifications;

/// <summary> . </summary>
public class NotDeletedByIdSpecification<T, TId> : Specification<T>
    where T : class, IEntity<TId>, IEntityAuditableLogicalRemove where TId : notnull
{
    private readonly TId _id;

    /// <summary> . </summary>
    public NotDeletedByIdSpecification(TId id)
    {
        _id = id;
    }

    /// <summary> . </summary>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return new AndSpecification<T>(new NotDeletedSpecification<T>(), new EntityByIdSpecification<T, TId>(_id))
            .SatisfiedBy();
    }
}
