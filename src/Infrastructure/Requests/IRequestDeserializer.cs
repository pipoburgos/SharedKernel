namespace SharedKernel.Infrastructure.Requests;

/// <summary> Request deserializer. </summary>
public interface IRequestDeserializer
{
    /// <summary> Deserialize request. </summary>
    Request Deserialize(string body);
}
