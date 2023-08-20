using SharedKernel.Application.Serializers;
using System.IO;
using System.Runtime.Serialization;

namespace SharedKernel.Infrastructure.Serializers;

/// <summary>  </summary>
public class BinarySerializer : IBinarySerializer
{
    /// <summary>  </summary>
    public byte[] Serialize<T>(T value)
    {
        if (value == null)
            return null;

        using var ms = new MemoryStream();
        var serializer = new DataContractSerializer(typeof(T));
        serializer.WriteObject(ms, value);
        return ms.ToArray();
    }

    /// <summary>  </summary>
    public T Deserialize<T>(byte[] value)
    {
        if (value == null)
            return default;

        using var memStream = new MemoryStream(value);
        var serializer = new DataContractSerializer(typeof(T));
        var obj = (T)serializer.ReadObject(memStream);
        return obj;
    }
}
