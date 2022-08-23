using SharedKernel.Domain.Events;
using System.Collections.Generic;

namespace SharedKernel.Integration.Tests.Events.Serialization
{
    public partial class DomainEventExtensionsToPrimitivesTests
    {
        public class UserCreated : DomainEvent
        {
            public UserCreated(Gender gender, string aggregateId, string eventId = default, string occurredOn = default) : base(aggregateId, eventId, occurredOn)
            {
                Gender = gender;
            }

            public override string GetEventName()
            {
                return "toPrimitives.userCreated";
            }

            public Gender Gender { get; }

            public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
            {
                var gender = GetEnumFromBody<Gender>(body, nameof(Gender));

                return new UserCreated(gender, aggregateId, eventId, occurredOn);
            }
        }
    }
}