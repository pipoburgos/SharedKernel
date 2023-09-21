using SharedKernel.Application.Security;
using SharedKernel.Application.Serializers;
// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal class RequestSerializer : IRequestSerializer
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IIdentityService? _identityService;

    /// <summary> Constructor. </summary>
    public RequestSerializer(IJsonSerializer jsonSerializer, IIdentityService? identityService = default)
    {
        _jsonSerializer = jsonSerializer;
        _identityService = identityService;
    }

    /// <summary>  </summary>
    public string Serialize(Request request)
    {
        if (request == default!)
            return string.Empty;

        var attributes = request.ToPrimitives();

        var domainClaims = _identityService?.User?.Claims
            .Select(c => new RequestClaim(c.Type, c.Value))
            .ToList();

        return _jsonSerializer.Serialize(new Dictionary<string, Dictionary<string, object>>
        {
            {
                RequestExtensions.Headers, new Dictionary<string, object?>
                {
                    {RequestExtensions.Claims, domainClaims},
                    {RequestExtensions.Authorization, _identityService?.GetKeyValue("Authorization")}
                }!
            },
            {
                RequestExtensions.Data, new Dictionary<string, object>
                {
                    {RequestExtensions.Id, request.RequestId},
                    {RequestExtensions.Type, request.GetUniqueName()},
                    {RequestExtensions.OccurredOn, request.OccurredOn},
                    {RequestExtensions.Attributes, attributes}
                }
            },
            {
                RequestExtensions.Meta, new Dictionary<string, object>()
            }
        }, NamingConvention.PascalCase);
    }
}
