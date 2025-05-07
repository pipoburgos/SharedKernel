namespace SharedKernel.Application.Serializers;

/// <summary> . </summary>
public interface IJsonSerializer
{
    /// <summary> . </summary>
    T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary> . </summary>
    /// <returns></returns>
    T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary> Deserialize <paramref name="stream"/> to an instance of <paramref name="type"/> </summary>
    object Deserialize(Type type, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary> Deserialize <paramref name="stream"/> to an instance of <typeparamref name="T" /></summary>
    T? Deserialize<T>(Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <inheritdoc cref="Deserialize"/>
    Task<object> DeserializeAsync(Type type, Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="Deserialize"/>
    Task<T?> DeserializeAsync<T>(Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase,
        CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary>
    /// Serialize an instance of <typeparamref name="T"/> to <paramref name="stream"/>
    /// </summary>
    /// <param name="data">The instance of <typeparamref name="T"/> that we want to serialize.</param>
    /// <param name="stream">The stream to serialize to.</param>
    /// <param name="namingConvention"></param>
    /// satisfy this hint, including the default serializer that is shipped with 8.0.
    void Serialize<T>(T data, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <inheritdoc cref="Serialize{T}"/>
    Task SerializeAsync<T>(T data, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase,
        CancellationToken cancellationToken = default);
}
