namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadSpecificationRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
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
