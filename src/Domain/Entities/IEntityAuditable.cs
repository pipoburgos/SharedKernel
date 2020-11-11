using System;

namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes
    /// </summary>
    public interface IEntityAuditable
    {
        Guid CreatedBy { get; }

        DateTime CreatedAt { get; }

        Guid? LastModifiedBy { get; }

        DateTime? LastModifiedAt { get; }

        void Create(DateTime createdAt, Guid createdBy);

        void Change(DateTime lastModifiedAt, Guid lastModifiedBy);
    }
}
