using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Events
{
    public class DomainEventMediator
    {
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, object> _domainEventSubscribers = new Dictionary<string, object>();

        public DomainEventMediator(
            ExecuteMiddlewaresService executeMiddlewaresService,
            IServiceProvider serviceProvider)
        {
            _executeMiddlewaresService = executeMiddlewaresService;
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteOn(DomainEvent @event, List<string> eventSubscribers, CancellationToken cancellationToken)
        {
            foreach (var eventSubscriber in eventSubscribers)
            {
                await _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken);

                using var scope = _serviceProvider.CreateScope();
                var subscriber = _domainEventSubscribers.ContainsKey(eventSubscriber)
                    ? _domainEventSubscribers[eventSubscriber]
                    : SubscribeFor(eventSubscriber, scope);
                await((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
            }
        }
        public async Task ExecuteOn(DomainEvent @event, string eventSubscriber, CancellationToken cancellationToken)
        {
            await _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            var subscriber = _domainEventSubscribers.ContainsKey(eventSubscriber)
                ? _domainEventSubscribers[eventSubscriber]
                : SubscribeFor(eventSubscriber, scope);

            await ((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
        }

        private object SubscribeFor(string queue, IServiceScope scope)
        {
            var queueParts = queue.Split('.');
            var subscriberName = ToCamelFirstUpper(queueParts.Last());

            var t = ReflectionHelper.GetType(subscriberName);

            var subscriber = scope.ServiceProvider.GetRequiredService(t);
            _domainEventSubscribers.Add(queue, subscriber);
            return subscriber;
        }

        private string ToCamelFirstUpper(string text)
        {
            var textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).TextInfo;
            return textInfo.ToTitleCase(text).Replace("_", string.Empty);
        }
    }
}
