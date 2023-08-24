using SharedKernel.Domain.Events;

namespace SharedKernel.Integration.Tests.Events.Serialization
{
    public partial class DomainEventExtensionsToPrimitivesTests
    {
        public class UserCreatedDateTimeNullable : DomainEvent
        {
            public UserCreatedDateTimeNullable(DateTime? dateTime, string aggregateId, string eventId = default, string occurredOn = default) : base(aggregateId, eventId, occurredOn)
            {
                DateTime = dateTime;
            }

            public override string GetEventName()
            {
                return "toPrimitives.userCreatedDateTimeNullable";
            }


            public DateTime? DateTime { get; }

            public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
            {
                DateTime? dateTime = default;
                if (!string.IsNullOrWhiteSpace(body[nameof(DateTime)]))
                    dateTime = ConvertToDateTime(body, nameof(DateTime));

                return new UserCreatedDateTimeNullable(dateTime, aggregateId, eventId, occurredOn);
            }
        }
    }
}