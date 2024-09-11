namespace SharedKernel.Domain.Repositories.Read;

/// <summary> . </summary>
public interface IReadAllRepository<TAggregate> : IBaseRepository where TAggregate : IAggregateRoot
{
    /// <summary> . </summary>
    List<TAggregate> GetAll();

    /// <summary> . </summary>
    bool Any();

    /// <summary> . </summary>
    bool NotAny();
}
