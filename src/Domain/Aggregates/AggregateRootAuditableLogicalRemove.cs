using System;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Aggregates
{
    public abstract class AggregateRootAuditableLogicalRemove<TKey> : AggregateRootAuditable<TKey>, IEntityAuditableLogicalRemove
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