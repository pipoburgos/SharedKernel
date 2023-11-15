using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Security.Cryptography;

public class EncryptionHexHelperTests
{
    [Fact]
    public void EncryptionHexHelperEncrypt()
    {
        var x = new Md5Encryptor().Encrypt("Texto de prueba", "TemplateWebApiSecretKey");

        Assert.NotNull(x);
        Assert.Equal("7D66AA9DF56FB2F689DAA097A8C58DB8", x);
    }

    [Fact]
    public void EncryptionHexHelperDecrypt()
    {
        var x = new Md5Encryptor().Decrypt("7D66AA9DF56FB2F689DAA097A8C58DB8", "TemplateWebApiSecretKey");

        Assert.NotNull(x);
        Assert.Equal("Texto de prueba", x);
    }
}