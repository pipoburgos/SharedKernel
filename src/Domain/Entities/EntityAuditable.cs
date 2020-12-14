using System;

namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// An entity with creation and modification audit
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityAuditable<TKey> : Entity<TKey>, IEntityAuditable
    {
        #region Constructor

        /// <summary>
        /// Entity constructor for ORMs
        /// </summary>
        protected EntityAuditable()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="createdAt">Creation Date</param>
        /// <param name="createdBy">Creation user identifier</param>
        protected EntityAuditable(TKey id, DateTime createdAt, Guid createdBy) : base(id)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        #endregion

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