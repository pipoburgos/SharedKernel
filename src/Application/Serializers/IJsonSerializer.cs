namespace SharedKernel.Application.Serializers;

/// <summary> . </summary>
public interface IJsonSerializer
{
    /// <summary> . </summary>
    string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary> . </summary>
    /// <returns></returns>
    T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase);

    /// <summary> . </summary>
    T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase);
}
