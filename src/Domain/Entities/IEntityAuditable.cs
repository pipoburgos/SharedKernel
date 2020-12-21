using System;

namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes
    /// </summary>
    public interface IEntityAuditable
    {
        /// <summary>
        /// 
        /// </summary>
        Guid CreatedBy { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid? LastModifiedBy { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? LastModifiedAt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdAt"></param>
        /// <param name="createdBy"></param>
        void Create(DateTime createdAt, Guid createdBy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastModifiedAt"></param>
        /// <param name="lastModifiedBy"></param>
        void Change(DateTime lastModifiedAt, Guid lastModifiedBy);
    }
}
