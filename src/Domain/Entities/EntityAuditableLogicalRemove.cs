namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityAuditableLogicalRemove<TKey> : EntityAuditable<TKey>, IEntityAuditableLogicalRemove
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Guid? DeletedBy { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DeletedAt { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedAt"></param>
        /// <param name="deletedBy"></param>
        public virtual void Delete(DateTime deletedAt, Guid deletedBy)
        {
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Restore()
        {
            DeletedBy = null;
            DeletedAt = null;
        }

        #endregion
    }
}