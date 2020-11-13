namespace SharedKernel.Application.Serializers
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T value);

        T Deserialize<T>(byte[] value);
    }
}
