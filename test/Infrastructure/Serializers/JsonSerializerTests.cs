using SharedKernel.Infrastructure.NetJson;
using SharedKernel.Infrastructure.Newtonsoft;

namespace SharedKernel.Integration.Tests.Serializers;

public class JsonSerializerTests
{
    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record User(string Name, DateTime Updated);

    [Fact]
    public void NetJsonSerializerTestUtcDate()
    {
        // Arrange
        const string userText = "{\"name\":\"Joe\",\"updated\":\"2022-10-31T09:00:00.000Z\"}";

        // Act
        var user = new NetJsonSerializer().Deserialize<User>(userText);

        // Assert
        user.Updated.Should().Be(new DateTime(2022, 10, 31, 9, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public void NewtonsoftSerializerTestUtcDate()
    {
        // Arrange
        const string userText = "{\"name\":\"Joe\",\"updated\":\"2022-10-31T09:00:00.000Z\"}";

        // Act
        var user = new NewtonsoftSerializer().Deserialize<User>(userText);

        // Assert
        user.Updated.Should().Be(new DateTime(2022, 10, 31, 9, 0, 0, DateTimeKind.Utc));
    }
}
