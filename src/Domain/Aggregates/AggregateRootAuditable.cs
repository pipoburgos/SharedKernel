using System;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Aggregates
{
    public abstract class AggregateRootAuditable<TKey> : AggregateRoot<TKey>, IEntityAuditable
    {
        #region Properties

        public Guid CreatedBy { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Guid? LastModifiedBy { get; private set; }

        public DateTime? LastModifiedAt { get; private set; }

        #endregion

        public void Create(DateTime createdAt, Guid createdBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        public void Change(DateTime lastModifiedAt, Guid lastModifiedBy)
        {
            LastModifiedAt = lastModifiedAt;
            LastModifiedBy = lastModifiedBy;
        }
    }
}