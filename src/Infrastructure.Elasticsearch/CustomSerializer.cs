using Elastic.Transport;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.NetJson;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Elasticsearch;

internal class CustomElasticsearchSerializer : Serializer
{
    private readonly JsonSerializerOptions _options;

    public CustomElasticsearchSerializer()
    {
        _options = NetJsonSerializer.GetOptions(NamingConvention.NoAction);
    }

    public override object? Deserialize(Type type, Stream stream)
    {
        return JsonSerializer.Deserialize(stream, type, _options);
    }

    public override T Deserialize<T>(Stream stream)
    {
        return JsonSerializer.Deserialize<T>(stream, _options);
    }

    public override ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.DeserializeAsync(stream, type, _options, cancellationToken);
    }

    public override ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);
    }

    public override void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
    {
        JsonSerializer.Serialize(stream, data, _options);
    }

    public override Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, data, _options, cancellationToken);
    }
}
