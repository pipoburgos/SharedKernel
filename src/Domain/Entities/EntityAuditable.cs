using System;

namespace SharedKernel.Domain.Entities
{
    public abstract class EntityAuditable<TKey> : Entity<TKey>, IEntityAuditable
    {
        protected EntityAuditable() { }

        // ReSharper disable once UnusedParameter.Local
        [Obsolete("Eliminar parmetro")]
        protected EntityAuditable(bool force) { }

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