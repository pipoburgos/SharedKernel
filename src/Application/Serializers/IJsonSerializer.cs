namespace SharedKernel.Application.Serializers
{
    public interface IJsonSerializer
    {
        string Serialize(object value);

        T Deserialize<T>(string value);

        T DeserializeAnonymousType<T>(string value, T obj);
    }
}
