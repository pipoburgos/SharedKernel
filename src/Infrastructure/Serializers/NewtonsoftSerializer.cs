using Newtonsoft.Json;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public class NewtonsoftSerializer : IJsonSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
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
            return JsonConvert.DeserializeAnonymousType(value, obj);
        }
    }
}
