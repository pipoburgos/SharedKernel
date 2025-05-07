//// Licensed to Elasticsearch B.V under one or more agreements.
//// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
//// See the LICENSE file in the project root for more information

//using Elastic.Transport;
//using System.Text.Json;

//namespace SharedKernel.Infrastructure.Elasticsearch;

///// <summary>
///// An abstract implementation of a transport <see cref="Serializer"/> which serializes using the Microsoft
///// <c>System.Text.Json</c> library.
///// </summary>
//public abstract class SystemTextJsonSerializer2 : Serializer
//{
//    private readonly JsonSerializerOptions _options;
//    private readonly JsonSerializerOptions _indentedOptions;

//    /// <summary>
//    /// An abstract implementation of a transport <see cref="Serializer"/> which serializes using the Microsoft
//    /// <c>System.Text.Json</c> library.
//    /// </summary>
//    protected SystemTextJsonSerializer2(IJsonSerializerOptionsProvider? provider = null)
//    {
//        provider ??= new TransportSerializerOptionsProvider();
//        _options = provider.CreateJsonSerializerOptions();
//        _indentedOptions = new JsonSerializerOptions(_options)
//        {
//            WriteIndented = true,
//        };
//    }

//    #region Serializer

//    /// <inheritdoc />
//    public override T Deserialize<T>(Stream stream)
//    {
//        if (TryReturnDefault(stream, out T deserialize))
//            return deserialize;

//        return JsonSerializer.Deserialize<T>(stream, GetJsonSerializerOptions());
//    }

//    /// <inheritdoc />
//    public override object? Deserialize(Type type, Stream stream)
//    {
//        if (TryReturnDefault(stream, out object deserialize))
//            return deserialize;

//        return JsonSerializer.Deserialize(stream, type, GetJsonSerializerOptions());
//    }

//    /// <inheritdoc />
//    public override ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
//    {
//        if (TryReturnDefault(stream, out T deserialize))
//            return new ValueTask<T>(deserialize);

//        return JsonSerializer.DeserializeAsync<T>(stream, GetJsonSerializerOptions(), cancellationToken);
//    }

//    /// <inheritdoc />
//    public override ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
//    {
//        if (TryReturnDefault(stream, out object deserialize))
//            return new ValueTask<object?>(deserialize);

//        return JsonSerializer.DeserializeAsync(stream, type, GetJsonSerializerOptions(), cancellationToken);
//    }

//    /// <inheritdoc />
//    public override void Serialize<T>(T data, Stream writableStream,
//        SerializationFormatting formatting = SerializationFormatting.None) =>
//        JsonSerializer.Serialize(writableStream, data, GetJsonSerializerOptions(formatting));

//    /// <inheritdoc />
//    public override Task SerializeAsync<T>(T data, Stream stream,
//        SerializationFormatting formatting = SerializationFormatting.None,
//        CancellationToken cancellationToken = default) =>
//        JsonSerializer.SerializeAsync(stream, data, GetJsonSerializerOptions(formatting), cancellationToken);

//    #endregion Serializer

//    /// <summary>
//    /// Returns the <see cref="JsonSerializerOptions"/> for this serializer, based on the given <paramref name="formatting"/>.
//    /// </summary>
//    /// <param name="formatting">The serialization formatting.</param>
//    /// <returns>The requested <see cref="JsonSerializerOptions"/>.</returns>
//    protected internal JsonSerializerOptions GetJsonSerializerOptions(SerializationFormatting formatting = SerializationFormatting.None) =>
//        formatting is SerializationFormatting.None ? _options : _indentedOptions;

//    private static bool TryReturnDefault<T>(Stream? stream, out T deserialize)
//    {
//        deserialize = default;
//        return stream is null || stream == Stream.Null || (stream.CanSeek && stream.Length == 0);
//    }
//}
