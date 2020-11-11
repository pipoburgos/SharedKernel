using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Events
{
    public interface IDomainEventSubscriberBase
    {
        Task On(DomainEvent @event, CancellationToken cancellationToken);
    }
}