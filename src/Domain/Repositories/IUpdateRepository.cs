namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IUpdateRepository<in TAggregate> where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    void Update(TAggregate aggregate);

    /// <summary>  </summary>
    void UpdateRange(IEnumerable<TAggregate> aggregates);
}
