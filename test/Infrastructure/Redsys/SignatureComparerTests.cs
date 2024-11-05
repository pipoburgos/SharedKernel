using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Services;

namespace SharedKernel.Integration.Tests.Redsys;

public class SignatureComparerTests
{
    private readonly ISignatureComparer _signatureComparer;
    public SignatureComparerTests()
    {
        _signatureComparer = new SignatureComparer();
    }

    [Fact]
    public void ValidateResponseSignature_ShouldReturnsTrue_WhenSignaturesAreEqual()
    {
        var result = _signatureComparer.ValidateResponseSignature("aaaa", "aaaa");
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateResponseSignature_ShouldReturnsFalse_WhenSignaturesAreDistinct()
    {
        var result = _signatureComparer.ValidateResponseSignature("aaaa", "bbb");
        result.Should().BeFalse();
    }

    [Fact]
    public void ValidateResponseSignature_ShouldReturnsTrue_WhenSignaturesAreEqualOnUTF8Mode()
    {
        var result = _signatureComparer.ValidateResponseSignature("oUIoxu1a8j8Cih01LvIfO46+yUbh3JjjAbj/y8+rlWQ=", "oUIoxu1a8j8Cih01LvIfO46-yUbh3JjjAbj_y8-rlWQ=");
        result.Should().BeTrue();
    }
}