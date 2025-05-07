//using Elastic.Transport;
//using SharedKernel.Application.Serializers;

//namespace SharedKernel.Infrastructure.Elasticsearch;

//internal class CustomElasticsearchSerializer : Serializer
//{
//    private readonly IJsonSerializer _jsonSerializer;

//    public CustomElasticsearchSerializer(IJsonSerializer jsonSerializer)
//    {
//        _jsonSerializer = jsonSerializer;
//    }

//    public override object Deserialize(Type type, Stream stream)
//    {
//        return _jsonSerializer.Deserialize(type, stream, NamingConvention.SnakeCase);
//    }

//    public override T Deserialize<T>(Stream stream)
//    {
//        return _jsonSerializer.Deserialize<T>(stream, NamingConvention.SnakeCase);
//    }

//    public override async ValueTask<object> DeserializeAsync(Type type, Stream stream,
//        CancellationToken cancellationToken = default)
//    {
//        return await _jsonSerializer.DeserializeAsync(type, stream, NamingConvention.SnakeCase, cancellationToken);
//    }

//    public override async ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
//    {
//        return await _jsonSerializer.DeserializeAsync<T>(stream, NamingConvention.SnakeCase, cancellationToken);
//    }

//    public override void Serialize<T>(T data, Stream stream,
//        SerializationFormatting formatting = SerializationFormatting.None)
//    {
//        _jsonSerializer.Serialize(data, stream, NamingConvention.SnakeCase);
//    }

//    public override Task SerializeAsync<T>(T data, Stream stream,
//        SerializationFormatting formatting = SerializationFormatting.None,
//        CancellationToken cancellationToken = default)
//    {
//        return _jsonSerializer.SerializeAsync(data, stream, NamingConvention.SnakeCase, cancellationToken);
//    }
//}
