namespace SharedKernel.Application.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="namingConvention"></param>
        /// <returns></returns>
        string Serialize(object value, NamingConvention namingConvention = NamingConvention.CamelCase);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="namingConvention"></param>
        /// <returns></returns>
        T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        /// <param name="namingConvention"></param>
        /// <returns></returns>
        T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase);
    }
}
