using System;

namespace SharedKernel.Domain.Entities
{
    public interface IEntityAuditableLogicalRemove : IEntityAuditable
    {
        Guid? DeletedBy { get; }

        DateTime? DeletedAt { get; }

        void Delete(DateTime deletedAt, Guid userId);

        void Restore();
    }
}
