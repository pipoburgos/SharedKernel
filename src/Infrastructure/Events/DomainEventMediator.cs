using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Application.Events;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventMediator : IDomainEventMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public DomainEventMediator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="event"></param>
        /// <param name="eventSubscriber"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task ExecuteOn(string body, DomainEvent @event, string eventSubscriber, CancellationToken cancellationToken)
        {
            var queueParts = eventSubscriber.Split('.');
            var subscriberName = ToCamelFirstUpper(queueParts.Last());

            var subscriberType = ReflectionHelper.GetType(subscriberName);

            using var scope = _serviceScopeFactory.CreateScope();

            var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
            AddIdentity(body, httpContextAccessor);

            var subscriber = scope.ServiceProvider.GetRequiredService(subscriberType);
            await ((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
        }

        private static string ToCamelFirstUpper(string text)
        {
            var textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).TextInfo;
            return textInfo.ToTitleCase(text).Replace("_", string.Empty);
        }

        private static void AddIdentity(string body, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == default)
                return;

            var eventData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(body);
            if (eventData == default)
                throw new ArgumentException(nameof(body));

            var headers = eventData["headers"];
            if (headers == default)
                return;

            var domainClaimsString = headers["claims"]?.ToString();
            if (domainClaimsString == default)
                return;

            var domainClaims = JsonConvert.DeserializeObject<List<DomainClaim>>(domainClaimsString!);
            if (domainClaims == default || !domainClaims.Any())
                return;

#if !NET461 && !NETSTANDARD
            httpContextAccessor.HttpContext ??= new DefaultHttpContext();
#endif

            httpContextAccessor.HttpContext.User =
                new ClaimsPrincipal(new ClaimsIdentity(domainClaims.Select(dc => new Claim(dc.Type, dc.Value))));
        }
    }
}
