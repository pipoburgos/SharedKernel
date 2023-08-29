namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface ICreateRepository<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    void Add(TAggregate aggregate);

    /// <summary>  </summary>
    void AddRange(IEnumerable<TAggregate> aggregates);
}
