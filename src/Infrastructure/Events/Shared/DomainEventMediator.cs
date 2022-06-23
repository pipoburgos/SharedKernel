using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventMediator : IDomainEventMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDomainEventJsonDeserializer _deserializer;
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

        /// <summary>
        /// Constructor
        /// </summary>
        public DomainEventMediator(
            IServiceScopeFactory serviceScopeFactory,
            IDomainEventJsonDeserializer deserializer,
            IExecuteMiddlewaresService executeMiddlewaresService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _deserializer = deserializer;
            _executeMiddlewaresService = executeMiddlewaresService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSerialized"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteDomainSubscribers(string eventSerialized, CancellationToken cancellationToken)
        {
            var eventDeserialized = _deserializer.Deserialize(eventSerialized);
            return Task.WhenAll(
                DomainEventSubscriberInformationService
                    .GetAllEventsSubscribers(eventDeserialized)
                    .Select(subscriber =>
                        ExecuteOn(eventSerialized, eventDeserialized, subscriber, cancellationToken)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="event"></param>
        /// <param name="eventSubscriber"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task ExecuteOn(string body, DomainEvent @event, Type eventSubscriber, CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, ct) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
                AddIdentity(body, httpContextAccessor);

                var subscriber = scope.ServiceProvider.GetRequiredService(eventSubscriber);
                return ((IDomainEventSubscriberBase)subscriber).On(req, ct);
            });
        }

        private static void AddIdentity(string body, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == default)
                return;

            var eventData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(body);
            if (eventData == default)
                throw new ArgumentException(nameof(eventData));

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
