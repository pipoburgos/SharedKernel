using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities
{
    public class FileEntity : AggregateRoot<string>
    {
        public static FileEntity Create(string path, byte[] data)
        {
            return new FileEntity
            {
                Id = path,
                Contents = data
            };
        }

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

        public string Name { get; private set; }

        public string Extension { get; private set; }

        public string ContentType { get; private set; }

        public byte[] Contents { get; private set; }

        public DirectoryEntity ParentDirectory { get; set; }
    }
}
