namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IDeleteRepository<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    void Remove(TAggregate aggregate);

    /// <summary>  </summary>
    void RemoveRange(IEnumerable<TAggregate> aggregate);
}
