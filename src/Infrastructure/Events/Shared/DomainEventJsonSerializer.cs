using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public DomainEventJsonSerializer(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor;
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

            var domainClaims = _httpContextAccessor?.HttpContext?.User.Claims
                .Select(c => new DomainClaim(c.Type, c.Value))
                .ToList();

            var authorizationHeader = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];

            return JsonSerializer.Serialize(new Dictionary<string, Dictionary<string, object>>
            {
                {"headers", new Dictionary<string, object>
                    {
                        {"claims", domainClaims},
                        {"authorization", authorizationHeader}
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