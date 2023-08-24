namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class FileEntity : AggregateRoot<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FileEntity Create(string path, byte[] data)
        {
            return new FileEntity
            {
                Id = path,
                Contents = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="extension"></param>
        /// <param name="contentType"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static FileEntity Create(string id, string name, string extension, string contentType, byte[] contents)
        {
            return new FileEntity
            {
                Id = id,
                Name = name,
                Extension = extension,
                ContentType = contentType,
                Contents = contents
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Contents { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DirectoryEntity ParentDirectory { get; set; }
    }
}
