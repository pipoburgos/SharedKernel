namespace SharedKernel.Application.Serializers;

/// <summary>  </summary>
public interface IBinarySerializer
{
    /// <summary>  </summary>
    byte[] Serialize<T>(T value);

    /// <summary>  </summary>
    T Deserialize<T>(byte[] value);
}
