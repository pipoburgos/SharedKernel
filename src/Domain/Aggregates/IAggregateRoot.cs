using System.Collections.Generic;
using SharedKernel.Domain.Events;

namespace SharedKernel.Domain.Aggregates
{
    public interface IAggregateRoot
    {
        List<DomainEvent> PullDomainEvents();
    }
}
