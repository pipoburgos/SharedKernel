using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Serializers;

public abstract class JsonSerializerTests : InfrastructureTestCase<FakeStartup>
{
    private readonly IJsonSerializer _jsonSerializer;

    public JsonSerializerTests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    [Theory]
    [InlineData(NamingConvention.NoAction)]
    [InlineData(NamingConvention.CamelCase)]
    [InlineData(NamingConvention.PascalCase)]
    [InlineData(NamingConvention.SnakeCase)]
    [InlineData(NamingConvention.TrainCase)]
    [InlineData(NamingConvention.KebapCase)]
    public void JsonSerializerAndDeserialize(NamingConvention convention)
    {
        // Arrange
        var user = UserMother.Create();

        // Act
        var userText = _jsonSerializer.Serialize(user, convention);
        var user2 = _jsonSerializer.Deserialize<User>(userText, convention);

        // Assert
        user2.Should().Be(user);
        user2.Name.Should().Be(user.Name);
        user2.NumberOfChildren.Should().Be(user.NumberOfChildren);
        user2.Birthdate.Should().Be(user.Birthdate);
        user2.Parent.Should().Be(user.Parent);
        user2.Emails.Should().Equal(user.Emails);
        user2.Addresses.Should().Equal(user.Addresses);
    }

    [Fact]
    public void SerializeAndDeserializeDictionary()
    {
        List<RequestClaim> claims = [new("A", "B"), new("C", "D")];

        var dic = new Dictionary<string, Dictionary<string, object>>
        {
            {
                RequestExtensions.Headers, new Dictionary<string, object?>
                {
                    {RequestExtensions.Claims, claims},
                    {RequestExtensions.Authorization, "Bearer xxxx"},
                }!
            },
            {
                RequestExtensions.Data, new Dictionary<string, object>
                {
                    {RequestExtensions.Id, Guid.NewGuid().ToString()},
                    {RequestExtensions.Type, "event"},
                    {RequestExtensions.OccurredOn, "Date"},
                    {RequestExtensions.Attributes, new Dictionary<string, string?>()},
                }
            },
            {
                RequestExtensions.Meta, new Dictionary<string, object>()
            },
        };


        var dicString = _jsonSerializer.Serialize(dic, NamingConvention.PascalCase);

        dicString.Should().NotBeNullOrWhiteSpace();
    }

    private class TestClass
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public TestClass(Guid id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }

    [Fact]
    public void Should_Deserialize_Valid_Json()
    {
        const string json = "{\"Id\":\"d44d7f32-7fa7-44f9-b12d-bcf32a9f35f7\",\"CreatedAt\":\"2025-02-20T00:00:00Z\"}";


        var result = _jsonSerializer.Deserialize<TestClass>(json, NamingConvention.PascalCase);

        Assert.NotNull(result);
        Assert.Equal(new Guid("d44d7f32-7fa7-44f9-b12d-bcf32a9f35f7"), result.Id);
        Assert.Equal(new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc), result.CreatedAt);
    }

    [Fact]
    public void Should_Serialize_Valid_Object()
    {
        var obj = new TestClass(Guid.NewGuid(), DateTime.UtcNow);


        var json = _jsonSerializer.Serialize(obj, NamingConvention.PascalCase);

        Assert.Contains("\"Id\":", json);
        Assert.Contains("\"CreatedAt\":", json);
    }

    [Fact]
    public void Should_Throw_Exception_When_Invalid_Date()
    {
        const string json = "{\"Id\":\"d44d7f32-7fa7-44f9-b12d-bcf32a9f35f7\",\"CreatedAt\":\"InvalidDate\"}";


        Assert.ThrowsAny<Exception>(() => _jsonSerializer.Deserialize<TestClass>(json, NamingConvention.PascalCase));
    }
}
