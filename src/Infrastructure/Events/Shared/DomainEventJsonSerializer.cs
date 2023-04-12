using SharedKernel.Application.Security;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Events.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventJsonSerializer : IDomainEventJsonSerializer
    {
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identityService"></param>
        public DomainEventJsonSerializer(IIdentityService identityService = null)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public string Serialize(DomainEvent domainEvent)
        {
            if (domainEvent == null) return "";

            var attributes = domainEvent.ToPrimitives();

            attributes.Add("id", domainEvent.AggregateId);

            var domainClaims = _identityService?.User?.Claims
                .Select(c => new DomainClaim(c.Type, c.Value))
                .ToList();

            var authorizationHeader = _identityService?.Headers["Authorization"];

            return JsonSerializer.Serialize(new Dictionary<string, Dictionary<string, object>>
            {
                {"headers", new Dictionary<string, object>
                    {
                        {"claims", domainClaims},
                        {"authorization", authorizationHeader?.ToString()}
                    }},
                {"data", new Dictionary<string,object>
                    {
                        {"id" , domainEvent.EventId},
                        {"type", domainEvent.GetEventName()},
                        {"occurred_on", domainEvent.OccurredOn},
                        {"attributes", attributes}
                    }},
                {"meta", new Dictionary<string,object>()}
            });
        }
    }
}