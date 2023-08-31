namespace SharedKernel.Domain.Repositories.Delete;

/// <summary>  </summary>
public interface IDeleteRepository<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    void Remove(TAggregate aggregateRoot);

    /// <summary>  </summary>
    void RemoveRange(IEnumerable<TAggregate> aggregates);
}
