namespace SharedKernel.Domain.Repositories.Create;

/// <summary> . </summary>
public interface ICreateRepository<in TAggregate> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary> . </summary>
    void Add(TAggregate aggregateRoot);

    /// <summary> . </summary>
    void AddRange(IEnumerable<TAggregate> aggregates);
}
