using SharedKernel.Application.Reflection;
using SharedKernel.Application.Serializers;
using System.Reflection;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal class RequestDeserializer : IRequestDeserializer
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IRequestProviderFactory _requestProviderFactory;

    /// <summary>  </summary>
    public RequestDeserializer(
        IJsonSerializer jsonSerializer,
        IRequestProviderFactory requestProviderFactory)
    {
        _jsonSerializer = jsonSerializer;
        _requestProviderFactory = requestProviderFactory;
    }

    /// <summary>  </summary>
    public Request Deserialize(string body)
    {
        var eventData = _jsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(body);

        if (eventData == default)
            throw new ArgumentException(nameof(body));

        var data = eventData[RequestExtensions.Data];
        var attributesString = data[RequestExtensions.Attributes].ToString();

        var attributes = _jsonSerializer.Deserialize<Dictionary<string, string>>(attributesString!, NamingConvention.PascalCase);

        if (attributes == default)
            throw new ArgumentException(nameof(body));

        var x = data[RequestExtensions.Type].ToString();
        var requestType = _requestProviderFactory.Get(x!);

        var instance = ReflectionHelper.CreateInstance<Request>(requestType);

        if (attributes.TryGetValue(nameof(DomainEvent.AggregateId), out var attribute))
        {
            return ((DomainEvent)requestType
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(DomainEvent.FromPrimitives))
                ?.Invoke(instance, [
                    attribute,
                    attributes,
                    data[RequestExtensions.Id].ToString()!,
                    data[RequestExtensions.OccurredOn].ToString()!
                ])!)!;
        }

        return ((Request)requestType
            .GetTypeInfo()
            .GetDeclaredMethod(nameof(Request.FromPrimitives))
            ?.Invoke(instance, [
                attributes,
                data[RequestExtensions.Id].ToString()!,
                data[RequestExtensions.OccurredOn].ToString()!
            ])!)!;
    }
}
