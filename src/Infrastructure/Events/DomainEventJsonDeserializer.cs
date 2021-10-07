using Newtonsoft.Json;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// Domain event deserializer
    /// </summary>
    public class DomainEventJsonDeserializer : IDomainEventJsonDeserializer
    {
        /// <summary>
        /// Domain event deserializer
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public DomainEvent Deserialize(string body)
        {
            var eventData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(body);

            if (eventData == default)
                throw new ArgumentException(nameof(body));

            var data = eventData["data"];
            var attributesString = data["attributes"].ToString();

            var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(attributesString!);

            if (attributes == default)
                throw new ArgumentException(nameof(body));

            var domainEventType = DomainEventsInformation.ForName((string)data["type"]);

            var instance = ReflectionHelper.CreateInstance<DomainEvent>(domainEventType);

            var domainEvent = (DomainEvent)domainEventType
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(DomainEvent.FromPrimitives))
                ?.Invoke(instance, new object[]
                {
                    attributes["id"],
                    attributes,
                    data["id"].ToString(),
                    data["occurred_on"].ToString()
                });

            return domainEvent;
        }
    }
}