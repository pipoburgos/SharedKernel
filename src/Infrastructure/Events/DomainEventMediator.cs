using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events
{
    public class DomainEventMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DomainEventMediator(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteOn(DomainEvent @event, string eventSubscriber, CancellationToken cancellationToken)
        {
            var queueParts = eventSubscriber.Split('.');
            var subscriberName = ToCamelFirstUpper(queueParts.Last());

            var subscriberType = ReflectionHelper.GetType(subscriberName);

            using var scope = _serviceScopeFactory.CreateScope();
            var subscriber = scope.ServiceProvider.GetRequiredService(subscriberType);

            await ((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
        }

        private string ToCamelFirstUpper(string text)
        {
            var textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).TextInfo;
            return textInfo.ToTitleCase(text).Replace("_", string.Empty);
        }
    }
}
