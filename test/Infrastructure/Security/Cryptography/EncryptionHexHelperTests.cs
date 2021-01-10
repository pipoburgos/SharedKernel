using SharedKernel.Infrastructure.Security.Cryptography;
using Xunit;

namespace SharedKernel.Infraestructure.Tests.Security.Cryptography
{
    public class EncryptionHexHelperTests
    {
        [Fact]
        public void EncryptionHexHelperEncrypt()
        {
            var x = new EncryptionHexHelper().Encrypt("Texto de prueba");

            Assert.NotNull(x);
            Assert.Equal("7D66AA9DF56FB2F689DAA097A8C58DB8", x);
        }

        [Fact]
        public void EncryptionHexHelperDecrypt()
        {
            var x = new EncryptionHexHelper().Decrypt("7D66AA9DF56FB2F689DAA097A8C58DB8");

            Assert.NotNull(x);
            Assert.Equal("Texto de prueba", x);
        }
    }
}
