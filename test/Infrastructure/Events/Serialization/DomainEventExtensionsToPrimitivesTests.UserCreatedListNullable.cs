using SharedKernel.Domain.Events;

namespace SharedKernel.Integration.Tests.Events.Serialization;

public partial class DomainEventExtensionsToPrimitivesTests
{
    public class UserCreatedListNullable : DomainEvent
    {
        public UserCreatedListNullable(List<int?> ids, string aggregateId, string? eventId = default,
            string? occurredOn = default) : base(aggregateId, eventId, occurredOn)
        {
            Ids = ids;
        }

        public override string GetEventName()
        {
            return "toPrimitives.userCreatedListNullable";
        }


        public List<int?> Ids { get; }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
            string occurredOn)
        {
            var ids = new List<int?>();
            if (!string.IsNullOrWhiteSpace(body[nameof(Ids)]))
                ids = body[nameof(Ids)].Split(",")
                    .Select(e => string.IsNullOrWhiteSpace(e) ? null : (int?)int.Parse(e)).ToList();

            return new UserCreatedListNullable(ids, aggregateId, eventId, occurredOn);
        }
    }
}
