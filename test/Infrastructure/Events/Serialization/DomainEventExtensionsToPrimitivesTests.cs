using FluentAssertions;
using Newtonsoft.Json;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.Serialization
{
    public partial class DomainEventExtensionsToPrimitivesTests
    {
        [Fact]
        public void CastEnumToInt()
        {
            const Gender gender = Gender.Female;

            var @event = new UserCreated(gender, default, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreated.Gender)].Should().Be(gender.ToString("D"));
        }

        [Fact]
        public void CastEnumNullableToIntNotValue()
        {
            var @event = new UserCreatedGenderNullable(default, DateTime.Now, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreatedGenderNullable.GenderNullable)].Should().BeNull();
        }

        [Fact]
        public void CastEnumNullableToInt()
        {
            const Gender gender = Gender.Male;
            var @event = new UserCreatedGenderNullable(gender, DateTime.Now, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreatedGenderNullable.GenderNullable)].Should().Be(gender.ToString("D"));
        }

        [Fact]
        public void CastEnumNullableToInt2()
        {
            const Gender gender = Gender.Male;

            var @event = new UserCreatedGenderNullable(gender, DateTime.Now, Guid.NewGuid().ToString());

            var primitives = @event.ToPrimitives();

            primitives[nameof(UserCreatedGenderNullable.GenderNullable)].Should().Be(gender.ToString("D"));

            var serializer = new DomainEventJsonSerializer();
            var body = serializer.Serialize(@event);

            var eventData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(body);

            if (eventData == default)
                throw new ArgumentException(nameof(body));

            var data = eventData["data"];
            var attributesString = data["attributes"].ToString();

            var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(attributesString!);

            if (attributes == default)
                throw new ArgumentException(nameof(body));

            var domainEventType = typeof(UserCreatedGenderNullable);

            var instance = ReflectionHelper.CreateInstance<DomainEvent>(domainEventType);

            var x = domainEventType
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(DomainEvent.FromPrimitives))
                ?.Invoke(instance, new object[]
                {
                    attributes["id"],
                    attributes,
                    data["id"].ToString(),
                    data["occurred_on"].ToString()
                });

            if (x is UserCreatedGenderNullable evento2)
            {
                evento2.GenderNullable.Should().Be(gender);
                evento2.DateTime.Should().Be(@event.DateTime);
            }
        }
    }
}
