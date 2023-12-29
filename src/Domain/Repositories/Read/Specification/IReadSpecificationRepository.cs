namespace SharedKernel.Domain.Repositories.Read.Specification;

/// <summary>  </summary>
public interface IReadSpecificationRepository<TAggregateRoot> : IBaseRepository
    where TAggregateRoot : class, IAggregateRoot
{
    /// <summary>  </summary>
    List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec);

    /// <summary>  </summary>
    TAggregateRoot Single(ISpecification<TAggregateRoot> spec);

    /// <summary>  </summary>
    TAggregateRoot? SingleOrDefault(ISpecification<TAggregateRoot> spec);

    /// <summary>  </summary>
    bool Any(ISpecification<TAggregateRoot> spec);

    /// <summary>  </summary>
    bool NotAny(ISpecification<TAggregateRoot> spec);
}
