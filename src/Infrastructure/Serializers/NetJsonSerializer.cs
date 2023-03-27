using SharedKernel.Application.Serializers;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public class NetJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T DeserializeAnonymousType<T>(string value, T obj)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
