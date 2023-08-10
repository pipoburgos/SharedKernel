using SharedKernel.Application.Security;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Requests;
using SharedKernel.Infrastructure.Events.Shared;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal class RequestSerializer : IRequestSerializer
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IIdentityService _identityService;

    /// <summary> Constructor. </summary>
    public RequestSerializer(IJsonSerializer jsonSerializer, IIdentityService identityService = null)
    {
        _jsonSerializer = jsonSerializer;
        _identityService = identityService;
    }

    /// <summary>  </summary>
    public string Serialize(Request request)
    {
        if (request == default)
            return "";

        var attributes = request.ToPrimitives();

        var domainClaims = _identityService?.User?.Claims
            .Select(c => new DomainClaim(c.Type, c.Value))
            .ToList();

        return _jsonSerializer.Serialize(new Dictionary<string, Dictionary<string, object>>
        {
            {"headers", new Dictionary<string, object>
            {
                {"claims", domainClaims},
                {"authorization", _identityService?.GetKeyValue("Authorization")}
            }},
            {"data", new Dictionary<string,object>
            {
                {"id" , request.RequestId},
                {"type", request.GetUniqueName()},
                {"occurred_on", request.OccurredOn},
                {"attributes", attributes}
            }},
            {"meta", new Dictionary<string,object>()}
        });
    }
}
