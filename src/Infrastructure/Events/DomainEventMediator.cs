using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Security;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="identityService"></param>
        public DomainEventMediator(
            IServiceScopeFactory serviceScopeFactory,
            IIdentityService identityService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _identityService = identityService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="eventSubscriber"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task ExecuteOn(DomainEvent @event, string eventSubscriber, CancellationToken cancellationToken)
        {
            var queueParts = eventSubscriber.Split('.');
            var subscriberName = ToCamelFirstUpper(queueParts.Last());

            var subscriberType = ReflectionHelper.GetType(subscriberName);

            using var scope = _serviceScopeFactory.CreateScope();

            var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor != default && _identityService.IsAuthenticated())
            {
#if !NET461 && !NETSTANDARD
                httpContextAccessor.HttpContext ??= new DefaultHttpContext();
#endif
#pragma warning disable S1066 // Collapsible "if" statements should be merged
                if (httpContextAccessor.HttpContext != default)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
                {
                    httpContextAccessor.HttpContext.User = _identityService.User;
                }
            }

            var subscriber = scope.ServiceProvider.GetRequiredService(subscriberType);
            await ((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
        }

        private static string ToCamelFirstUpper(string text)
        {
            var textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).TextInfo;
            return textInfo.ToTitleCase(text).Replace("_", string.Empty);
        }
    }
}
