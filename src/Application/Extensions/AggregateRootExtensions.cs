namespace SharedKernel.Application.Extensions;

/// <summary> AggregateRoot extensions. </summary>
public static class AggregateRootExtensions
{
    /// <summary> Pull all domain events. </summary>
    /// <param name="aggregateRoots"></param>
    /// <returns></returns>
    public static IEnumerable<DomainEvent> PullDomainEvents(this IEnumerable<IAggregateRoot> aggregateRoots)
    {
        return aggregateRoots.SelectMany(a => a.PullDomainEvents());
    }
}