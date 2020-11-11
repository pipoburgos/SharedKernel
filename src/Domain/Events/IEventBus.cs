namespace SharedKernel.Domain.Events
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;

    public interface IEventBus
    {
        Task Publish(DomainEvent @event, CancellationToken cancellationToken);

        Task Publish(List<DomainEvent> events, CancellationToken cancellationToken);
    }
}