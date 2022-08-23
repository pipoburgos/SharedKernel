using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;

namespace SharedKernel.Integration.Tests.Events.Serialization
{
    public partial class DomainEventExtensionsToPrimitivesTests
    {
        public class UserCreatedGenderNullable : DomainEvent
        {
            public UserCreatedGenderNullable(Gender? gender, DateTime dateTime, string aggregateId, string eventId = default, string occurredOn = default) : base(aggregateId, eventId, occurredOn)
            {
                GenderNullable = gender;
                DateTime = dateTime;
            }

            public override string GetEventName()
            {
                return "toPrimitives.userCreatedGenderNullable";
            }

            public Gender? GenderNullable { get; }

            public DateTime DateTime { get; }

            public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
            {
                Gender? gender = default;
                if (!string.IsNullOrWhiteSpace(body[nameof(GenderNullable)]))
                    gender = GetEnumFromBody<Gender>(body, nameof(GenderNullable));

                var dateTime = ConvertToDateTime(body, nameof(DateTime));

                return new UserCreatedGenderNullable(gender, dateTime, aggregateId, eventId, occurredOn);
            }
        }
    }
}