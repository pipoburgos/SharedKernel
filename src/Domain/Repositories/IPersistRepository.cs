namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPersistRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Rollback();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
