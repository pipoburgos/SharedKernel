using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys.Helpers;

public class Sha256Tests
{
    [Fact]
    public void HashHMAC_ShouldWork()
    {
        var result = new Sha256().HashHmac("abcdefg", "12345678901234");
        result.Should().Be("ljGdvKkYI4CzuasiUiLM81CAW1zIz31yo8lajNOxGz8=");
    }
}