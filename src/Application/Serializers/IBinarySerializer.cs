namespace SharedKernel.Application.Serializers;

/// <summary> . </summary>
public interface IBinarySerializer
{
    /// <summary> . </summary>
    byte[] Serialize<T>(T value) where T : notnull;

    /// <summary> . </summary>
    T? Deserialize<T>(byte[] value) where T : notnull;
}
