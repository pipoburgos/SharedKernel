namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class EntityByIdSpecification<T, TId> : Specification<T> where T : class, IEntity<TId> where TId : notnull
{
    private readonly TId _id;

    /// <summary>  </summary>
    public EntityByIdSpecification(TId id)
    {
        _id = id;
    }

    /// <summary>  </summary>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return e => e.Id.Equals(_id);
    }
}
