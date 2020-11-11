using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventBus(
            ExecuteMiddlewaresService executeMiddlewaresService,
            IServiceProvider serviceProvider)
        {
            _executeMiddlewaresService = executeMiddlewaresService;
            _serviceProvider = serviceProvider;
        }

        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return Publish(new List<DomainEvent> { @event }, cancellationToken);
        }

        public async Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            if (events == null)
                return;

            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var @event in events)
                {
                    _executeMiddlewaresService.Execute(@event);

                    var subscribers = GetSubscribers(@event, scope);

                    var tasks = new List<Task>();
                    foreach (var subscriber in subscribers)
                    {
                        tasks.Add(((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken));
                    }

                    await Task.WhenAll(tasks);
                }
            }
        }

        private static IEnumerable<object> GetSubscribers(DomainEvent @event, IServiceScope scope)
        {
            var eventType = @event.GetType();
            var subscriberType = typeof(DomainEventSubscriber<>).MakeGenericType(eventType);
            return scope.ServiceProvider.GetServices(subscriberType);
        }
    }
}