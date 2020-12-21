namespace SharedKernel.Application.UnitOfWorks
{
    /// <summary>
    /// Synchronous unit of work pattern
    /// </summary>
    public interface IUnitOfWork
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