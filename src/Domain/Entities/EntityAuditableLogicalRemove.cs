using System;

namespace SharedKernel.Domain.Entities
{
    public abstract class EntityAuditableLogicalRemove<TKey> : EntityAuditable<TKey>, IEntityAuditableLogicalRemove
    {
        #region Properties

        public Guid? DeletedBy { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        #endregion

        #region Public Methods

        public virtual void Delete(DateTime deletedAt, Guid deletedBy)
        {
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
        }

        public virtual void Restore()
        {
            DeletedBy = null;
            DeletedAt = null;
        }

        #endregion
    }
}