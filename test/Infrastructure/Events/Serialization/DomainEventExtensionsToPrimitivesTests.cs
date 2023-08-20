using FluentAssertions;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.NetJson;
using SharedKernel.Infrastructure.Requests;
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
        public void CastDatetTimeNullableToPrimitives()
        {
            var @event = new UserCreatedDateTimeNullable(default, default, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreatedDateTimeNullable.DateTime)].Should().BeNull();

            var x = SendToBus(@event);

            if (x is UserCreatedDateTimeNullable evento2)
            {
                evento2.DateTime.Should().BeNull();
            }
        }

        [Fact]
        public void CastDatetTimeNullableToPrimitivesWithValue()
        {
            var now = DateTime.Now;
            var @event = new UserCreatedDateTimeNullable(now, default, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreatedDateTimeNullable.DateTime)].Should().Be(now.ToString("O"));

            var x = SendToBus(@event);

            if (x is UserCreatedDateTimeNullable evento2)
            {
                evento2.DateTime.Should().Be(now);
            }
        }

        [Fact]
        public void CastListNullableNullableToPrimitives()
        {
            var ids = new List<int?> { 2, 3, default, 6, default, 2 };
            var @event = new UserCreatedListNullable(ids, default, Guid.NewGuid().ToString());

            var x = SendToBus(@event);

            if (x is UserCreatedListNullable evento2)
            {
                evento2.Ids.Should().Contain(ids);
            }
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
        public void CastStringEvent()
        {
            const string name = "Roberto Fernández";

            var @event = new UserCreatedName(name, Guid.NewGuid().ToString());

            var result = @event.ToPrimitives();

            result[nameof(UserCreatedName.Name)].Should().Be(name);
        }

        [Fact]
        public void CastEnumNullableToInt2()
        {
            const Gender gender = Gender.Male;

            var @event = new UserCreatedGenderNullable(gender, DateTime.Now, Guid.NewGuid().ToString());

            var primitives = @event.ToPrimitives();

            primitives[nameof(UserCreatedGenderNullable.GenderNullable)].Should().Be(gender.ToString("D"));

            var x = SendToBus(@event);

            if (x is UserCreatedGenderNullable evento2)
            {
                evento2.GenderNullable.Should().Be(gender);
                evento2.DateTime.Should().Be(@event.DateTime);
            }
        }

        private static object SendToBus(DomainEvent @event)
        {
            var jsonSerializer = new NetJsonSerializer();
            var serializer = new RequestSerializer(jsonSerializer);
            var body = serializer.Serialize(@event);

            var eventData = jsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(body);

            if (eventData == default)
                throw new ArgumentException(nameof(body));

            var data = eventData["data"];
            var attributesString = data["attributes"].ToString();

            var attributes = jsonSerializer.Deserialize<Dictionary<string, string>>(attributesString!);

            if (attributes == default)
                throw new ArgumentException(nameof(body));

            var instance = ReflectionHelper.CreateInstance<DomainEvent>(@event.GetType());

            var domainEvent = (DomainEvent)@event.GetType()
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(DomainEvent.FromPrimitives))
                ?.Invoke(instance, new object[]
                {
                    attributes[nameof(DomainEvent.AggregateId)],
                    attributes,
                    data["id"].ToString(),
                    data["occurred_on"].ToString()
                });

            return domainEvent;
        }
    }
}
