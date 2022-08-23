using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;
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
    internal class DomainEventMediator : IDomainEventMediator
    {
        private readonly ICustomLogger<DomainEventMediator> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDomainEventJsonDeserializer _deserializer;
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IDomainEventSubscriberProviderFactory _domainEventSubscriberProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DomainEventMediator(
            ICustomLogger<DomainEventMediator> logger,
            IServiceScopeFactory serviceScopeFactory,
            IDomainEventJsonDeserializer deserializer,
            IExecuteMiddlewaresService executeMiddlewaresService,
            IDomainEventSubscriberProviderFactory domainEventSubscriberProviderFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _deserializer = deserializer;
            _executeMiddlewaresService = executeMiddlewaresService;
            _domainEventSubscriberProviderFactory = domainEventSubscriberProviderFactory;
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
                _domainEventSubscriberProviderFactory
                    .GetSubscribers(eventDeserialized)
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
        public Task ExecuteOn(string body, DomainEvent @event, Type eventSubscriber,
            CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, async (req, ct) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
                AddIdentity(body, httpContextAccessor);

                var subscriber = scope.ServiceProvider.GetRequiredService(eventSubscriber);
                _logger.Info($"Executing {eventSubscriber.FullName} with data: {body}");
                await ((IDomainEventSubscriberBase)subscriber).On(req, ct);
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

            var domainClaimsString = headers["claims"]?.ToString();
            if (domainClaimsString == null)
                return;

            var domainClaims = JsonConvert.DeserializeObject<List<DomainClaim>>(domainClaimsString!);
            if (domainClaims == default || !domainClaims.Any())
                return;

#if !NET461 && !NETSTANDARD
            httpContextAccessor.HttpContext ??= new DefaultHttpContext();
#endif

            httpContextAccessor.HttpContext.User =
                new ClaimsPrincipal(new ClaimsIdentity(domainClaims.Select(dc => new Claim(dc.Type, dc.Value))));

            var authorization = headers["authorization"]?.ToString();
            if (!string.IsNullOrWhiteSpace(authorization))
                httpContextAccessor.HttpContext.Request.Headers.Add("Authorization", authorization);
        }
    }
}
