using SharedKernel.Domain.Events;

namespace SharedKernel.Domain.Tests.Users
{
    internal class UserCreated : DomainEvent
    {
        public UserCreated(Guid id, string name, string aggregateId, string eventId = null, string occurredOn = null) :
            base(aggregateId, eventId, occurredOn)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }

        public override string GetEventName()
        {
            return "user.created";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new UserCreated(new Guid(body[nameof(Id)]), body[nameof(Name)], aggregateId, eventId, occurredOn);
        }
    }
}
