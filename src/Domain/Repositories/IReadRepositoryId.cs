namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadRepository<out TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    TAggregate? GetById(TId key);

    /// <summary>  </summary>
    bool Any();

    /// <summary> </summary>
    bool NotAny();

    /// <summary>  </summary>
    bool Any(TId key);

    /// <summary>  </summary>
    bool NotAny(TId key);
}