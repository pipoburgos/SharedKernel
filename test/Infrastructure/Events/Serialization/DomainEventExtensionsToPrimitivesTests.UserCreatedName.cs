using SharedKernel.Domain.Events;

namespace SharedKernel.Integration.Tests.Events.Serialization
{
    public partial class DomainEventExtensionsToPrimitivesTests
    {
        public class UserCreatedName : DomainEvent
        {
            public UserCreatedName(string name, string aggregateId, string eventId = default, string occurredOn = default) : base(aggregateId, eventId, occurredOn)
            {
                Name = name;
            }

            public override string GetEventName()
            {
                return "toPrimitives.userCreatedName";
            }

            public string Name { get; }

            public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
            {
                return new UserCreatedName(body[nameof(Name)], aggregateId, eventId, occurredOn);
            }
        }
    }
}