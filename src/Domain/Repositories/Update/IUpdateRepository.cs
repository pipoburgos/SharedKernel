namespace SharedKernel.Domain.Repositories.Update;

/// <summary>  </summary>
public interface IUpdateRepository<in TAggregate> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary>  </summary>
    void Update(TAggregate aggregateRoot);

    /// <summary>  </summary>
    void UpdateRange(IEnumerable<TAggregate> aggregates);
}
