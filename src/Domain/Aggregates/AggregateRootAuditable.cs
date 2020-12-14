using System;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Aggregates
{
    /// <summary>
    /// Root aggregate with creation and modification audit
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AggregateRootAuditable<TKey> : AggregateRoot<TKey>, IEntityAuditable
    {
        #region Constructor

        /// <summary>
        /// Aggregate Root Auditable constructor for ORMs
        /// </summary>
        protected AggregateRootAuditable() { }

        /// <summary>
        /// Aggregate Root Auditable Constructor
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="createdAt">Creation Date</param>
        /// <param name="createdBy">Creation user identifier</param>
        protected AggregateRootAuditable(TKey id, DateTime createdAt, Guid createdBy) : base(id)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Creator user identifier
        /// </summary>
        public Guid CreatedBy { get; private set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Modifier user identifier
        /// </summary>
        public Guid? LastModifiedBy { get; private set; }

        /// <summary>
        /// Modification date
        /// </summary>
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