namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityAuditableLogicalRemove : IEntityAuditable
    {
        /// <summary>
        /// 
        /// </summary>
        Guid? DeletedBy { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? DeletedAt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedAt"></param>
        /// <param name="userId"></param>
        void Delete(DateTime deletedAt, Guid userId);

        /// <summary>
        /// 
        /// </summary>
        void Restore();
    }
}
