//using Elastic.Clients.Elasticsearch;
//using Elastic.Clients.Elasticsearch.Serialization;
//using System.Text.Json;

//namespace SharedKernel.Infrastructure.Elasticsearch;

//public class CustomSystemTextJsonSerializer : DefaultSourceSerializer
//{
//    public CustomSystemTextJsonSerializer(IElasticsearchClientSettings settings, Action<JsonSerializerOptions>? configureOptions = null) : base(settings, configureOptions)
//    {
//    }

//    public override T Deserialize<T>(Stream stream)
//    {
//        return base.Deserialize<T>(stream);
//    }

//    public override object? Deserialize(Type type, Stream stream)
//    {
//        return base.Deserialize(type, stream);
//    }

//    public override ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = new CancellationToken())
//    {
//        return base.DeserializeAsync<T>(stream, cancellationToken);
//    }

//    public override ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = new CancellationToken())
//    {
//        return base.DeserializeAsync(type, stream, cancellationToken);
//    }
//}
