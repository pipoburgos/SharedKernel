using SharedKernel.Application.Serializers;
using System.IO;
using System.Runtime.Serialization;

namespace SharedKernel.Infrastructure.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public class BinarySerializer : IBinarySerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T value)
        {
            if (value == null)
                return null;

            using var ms = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(ms, value);
            return ms.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] value)
        {
            if (value == null)
                return default;

            using var memStream = new MemoryStream(value);
            var serializer = new DataContractSerializer(typeof(T));
            var obj = (T)serializer.ReadObject(memStream);
            return obj;
        }

        ///// <summary>
        ///// Obsolete: Convert an Object to byte array
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public byte[] Serialize<T>(T obj)
        //{
        //    if (obj == null)
        //        return null;

        //    var bf = new BinaryFormatter();
        //    using (var ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        ///// <summary>
        ///// Obsolete: Convert a byte array to an Object
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="arrBytes"></param>
        ///// <returns></returns>
        //public T Deserialize<T>(byte[] arrBytes)
        //{
        //    var memStream = new MemoryStream();
        //    var binForm = new BinaryFormatter();
        //    memStream.Write(arrBytes, 0, arrBytes.Length);
        //    memStream.Seek(0, SeekOrigin.Begin);
        //    var obj = (T)binForm.Deserialize(memStream);

        //    return obj;
        //}
    }
}
