namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDirectoryRepositoryAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<string>> GetFilesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<string>> GetFileNamesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// ATTENTION, READ THE CONTENTS OF ALL FILES AND LOAD THEM IN MEMORY
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<FileEntity>> GetFilesEntitiesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task AddAsync(DirectoryEntity directory, CancellationToken cancellationToken);
    }
}
