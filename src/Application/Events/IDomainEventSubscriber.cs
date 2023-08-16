using SharedKernel.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Events;

/// <summary>  </summary>
public interface IDomainEventSubscriber<in T> where T : DomainEvent
{
    /// <summary>  </summary>
    Task On(T @event, CancellationToken cancellationToken);
}
