namespace SharedKernel.Domain.Repositories.Read;

/// <summary>  </summary>
public interface IReadOneRepository<out TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    TAggregate? GetById(TId id);

    /// <summary>  </summary>
    bool Any(TId id);

    /// <summary>  </summary>
    bool NotAny(TId id);
}