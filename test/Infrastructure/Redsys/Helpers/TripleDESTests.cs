using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys.Helpers;

public class TripleDesTests
{
    [Fact]
    public void Encrypt_ShouldWork()
    {
        var key = new Base64().DecodeFrom64("Mk9m98IfEblmPfrpsawt7BmxObt98Jev");
        var result = new TripleDes().Encrypt(key, "abcdefg");
        var base64Result = new Base64().EncodeTo64(result);
        base64Result.Should().Be("kQG+7RwPCZo=");
    }
}