using SharedKernel.Application.Serializers;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Serializers
{
    public class NetJsonSerializer : IJsonSerializer
    {
        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        public T DeserializeAnonymousType<T>(string value, T obj)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
