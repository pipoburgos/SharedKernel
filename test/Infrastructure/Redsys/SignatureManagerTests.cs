using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Services;
using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys;

public class SignatureManagerTests
{
    private readonly ISignatureManager _signatureManager;
    public SignatureManagerTests()
    {
        _signatureManager = new SignatureManager(new Base64(), new TripleDes(), new Sha256());
    }

    [Fact]
    public void GetSignature_ShouldWork()
    {
        const string merchantParameters = "eyJEc19NZXJjaGFudF9BbW91bnQiOiIxNDUiLCJEc19NZXJjaGFudF9PcmRlciI6IjE5OTkwMDAwMDAwQSIsIkRzX01lcmNoYW50X01lcmNoYW50Q29kZSI6Ijk5OTAwODg4MSIsIkRzX01lcmNoYW50X0N1cnJlbmN5IjoiOTc4IiwiRHNfTWVyY2hhbnRfVHJhbnNhY3Rpb25UeXBlIjoiMCIsIkRzX01lcmNoYW50X1Rlcm1pbmFsIjoiODcxIiwiRHNfTWVyY2hhbnRfTWVyY2hhbnRVUkwiOiIiLCJEc19NZXJjaGFudF9VcmxPSyI6IiIsIkRzX01lcmNoYW50X1VybEtPIjoiIn0=";
        const string merchantOrder = "19990000000A";
        const string merchantKey = "Mk9m98IfEblmPfrpsawt7BmxObt98Jev";

        var result = _signatureManager.GetSignature(merchantParameters, merchantOrder, merchantKey);
        result.Should().Be("MAlGASPeuqCw4K4ZMNIR343ljOoEAmH7B5woby1kcbs=");
    }

    [Fact]
    public void GetSignature_ShouldWork_2()
    {
        const string merchantParameters = "eyJEc19NZXJjaGFudF9BbW91bnQiOiIxNDUiLCJEc19NZXJjaGFudF9PcmRlciI6Ijk5OTkxMTExMTExMSIsIkRzX01lcmNoYW50X01lcmNoYW50Q29kZSI6Ijk5OTAwODg4MSIsIkRzX01lcmNoYW50X0N1cnJlbmN5IjoiOTc4IiwiRHNfTWVyY2hhbnRfVHJhbnNhY3Rpb25UeXBlIjoiMCIsIkRzX01lcmNoYW50X1Rlcm1pbmFsIjoiODcxIiwiRHNfTWVyY2hhbnRfTWVyY2hhbnRVUkwiOiIiLCJEc19NZXJjaGFudF9VcmxPSyI6IiIsIkRzX01lcmNoYW50X1VybEtPIjoiIn0=";
        const string merchantOrder = "999911111111";
        const string merchantKey = "Mk9m98IfEblmPfrpsawt7BmxObt98Jev";

        var result = _signatureManager.GetSignature(merchantParameters, merchantOrder, merchantKey);
        result.Should().Be("nGLmVWiI78Yf9fKUFh/70sSJ7S55idKI6FWgh2MkIDY=");
    }

}