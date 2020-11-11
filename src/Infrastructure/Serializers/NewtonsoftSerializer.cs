using Newtonsoft.Json;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Serializers
{
    public class NewtonsoftSerializer : IJsonSerializer
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public T DeserializeAnonymousType<T>(string value, T obj)
        {
            return JsonConvert.DeserializeAnonymousType(value, obj);
        }
    }
}
